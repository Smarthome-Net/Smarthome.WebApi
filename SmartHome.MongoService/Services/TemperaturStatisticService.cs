using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Extensions;
using SmartHome.Common.Helpers;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.MongoService.DbContext;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.MongoService.Services;

public class TemperaturStatisticService : ITemperatureStatisticService
{
    private readonly MongoDBContext _dbContext;

    public TemperaturStatisticService(MongoDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Chart<string, float> GetStatistic(StatisticRequest request)
    {
        var predicate = request.Scope.ToPredicate<Device>(
            (device, room) => device.Room == room,
            (device, room, name) => device.Room == room && device.Name == name);
        var temperatureQuery = _dbContext.TemperatureCollection.AsQueryable();
        var deviceQuery = _dbContext.DeviceCollection.AsQueryable();

        var baseResult = deviceQuery
            .Where(predicate)
            .Join(temperatureQuery.AsQueryable(),
                device => device.Id,
                temperature => temperature.DeviceId,
                (device, temperature) => new Temperature()
                {
                    Id = temperature.Id,
                    Value = temperature.Value,
                    RecordDateTime = temperature.RecordDateTime,
                    DeviceId = temperature.DeviceId,
                    Device = device,
                });

        var max = FilterMax(baseResult);
        var min = FilterMin(baseResult);
        var avg = FilterAverage(baseResult);

        return new Chart<string, float>
        {
            Name = request.Scope.Value,
            Series =
            [
                SeriesHelper.Create("min", min),
                SeriesHelper.Create("average", avg),
                SeriesHelper.Create("max", max),
            ],
        };
    }

    private static float FilterMax(IEnumerable<Temperature> data)
    {
        return data
            .OrderByDescending(item => item.Value)
            .Select(item => item.Value)
            .FirstOrDefault();
    }

    private static float FilterMin(IEnumerable<Temperature> data)
    {
        return data
            .OrderBy(item => item.Value)
            .Select(item => item.Value)
            .FirstOrDefault();
    }

    private static float FilterAverage(IEnumerable<Temperature> data)
    {
        return data.Average(i => i.Value);
    }
}
