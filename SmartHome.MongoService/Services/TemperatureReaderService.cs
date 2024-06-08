using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Extensions;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.MongoService.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.MongoService.Services;

public class TemperatureReaderService : ITemperatureReaderService
{
    private readonly MongoDBContext _dbContext;

    public TemperatureReaderService(MongoDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Chart<DateTimeOffset, float>> GetTemperature(TemperatureRequest request)
    {
        var predicate = request.Scope.ToPredicate();
        var keySelector = request.Scope.ToTemperatureKeySelector();
        var temperatureQuery = _dbContext.TemperatureCollection.AsQueryable();
        var deviceQuery = _dbContext.DeviceCollection.AsQueryable();

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
            .ToTimeSeriesChart(keySelector)
            .ApplayPaging(request.PageSetting);
    }
}
