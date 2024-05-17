using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Extensions;
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

    public Chart<NamedSeries> GetStatistic(StatisticRequest request)
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

        return new Chart<NamedSeries>
        {
            Name = request.Scope.Value,
            Series =
            [
                CreateNameSeries("min", min),
                CreateNameSeries("average", avg),
                CreateNameSeries("max", max),
            ],
        };
    }

    public static NamedSeries CreateNameSeries(string name, float value)
    {
        return new NamedSeries
        {
            Name = name,
            Value = value,
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
