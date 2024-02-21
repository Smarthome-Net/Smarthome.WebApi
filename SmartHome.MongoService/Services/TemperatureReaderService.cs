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
        var predicate = request.Scope.ToPredicate<Device>(
            (device, room) => device.Room == room, 
            (device, room, name) => device.Room == room && device.Name == name);
        var keySelector = request.Scope.ToTemperatureKeySelector();

        var data = QueryData(predicate);
        var pageSetting = request.PageSetting;

        return GroupData(keySelector, pageSetting, data);
    }

    private List<Temperature> QueryData(Func<Device, bool> predicate)
    {
        var temperatureQuery = _temperatureCollection.AsQueryable();
        var deviceQuery = _deviceCollection.AsQueryable();

        return deviceQuery
            .Where(predicate)
            .Join(temperatureQuery,
                device => device.Id,
                temperature => temperature.DeviceId,
                (device, temperature) => new Temperature()
                {
                    Id = temperature.Id,
                    Value = temperature.Value,
                    RecordDateTime = temperature.RecordDateTime,
                    DeviceId = temperature.DeviceId,
                    Device = device
                })
            .OrderByDescending(item => item.RecordDateTime)
            .ToList();
    }
}
