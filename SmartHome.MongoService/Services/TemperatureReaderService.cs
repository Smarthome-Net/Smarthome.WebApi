using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.DTO;
using SmartHome.Common.QueryHelper;
using SmartHome.MongoService.Provider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.MongoService.Services;

public class TemperatureReaderService : TemperatureQueryBase, ITemperatureReaderService
{
    private readonly IMongoCollection<Temperature> _temperatureCollection;
    private readonly IMongoCollection<Device> _deviceCollection;

    public TemperatureReaderService(MongoDBConnectionProvider mongoConnectionProvider) 
    {
        _temperatureCollection = mongoConnectionProvider.GetTemperatureCollection();
        _deviceCollection = mongoConnectionProvider.GetDeviceCollection();
    }

    public IEnumerable<Chart<TimeSeries>> GetTemperature(TemperatureRequest request)
    {
        Func<Temperature, bool> predicate = CreatePredicate(request.Scope, request.ScopeValue);
        Func<Temperature, string> keySelector = CreateKeySelector(request.Scope);

        var temperatureQuery = _temperatureCollection.AsQueryable();
        var deviceQuery = _deviceCollection.AsQueryable();
        
        var data = QueryData(predicate, deviceQuery, temperatureQuery);
        var pageSetting = request.PageSetting;
        pageSetting.Length = data.Count;

        return GroupData(keySelector, pageSetting, data);
    }

    private static List<Temperature> QueryData(Func<Temperature, bool> predicate, 
        IMongoQueryable<Device> deviceQuery, 
        IMongoQueryable<Temperature> temperatureQuery)
    {
        return temperatureQuery
            .Join(deviceQuery,
                p => p.DeviceId,
                o => o.Id,
                (p, o) => new Temperature()
                {
                    Id = p.Id,
                    Value = p.Value,
                    RecordDateTime = p.RecordDateTime,
                    DeviceId = p.DeviceId,
                    Device = o
                })
            .Where(predicate)
            .OrderByDescending(item => item.RecordDateTime)
            .ToList();
    }
}
