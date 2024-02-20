using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Extensions;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;
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
        Func<Temperature, bool> predicate = request.Scope.ToTemperaturePredicate();
        Func<Temperature, string> keySelector = request.Scope.ToTemperatureKeySelector();

        var data = QueryData(predicate);
        var pageSetting = request.PageSetting;

        return GroupData(keySelector, pageSetting, data);
    }

    private List<Temperature> QueryData(Func<Temperature, bool> predicate)
    {
        var temperatureQuery = _temperatureCollection.AsQueryable();
        var deviceQuery = _deviceCollection.AsQueryable();

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
