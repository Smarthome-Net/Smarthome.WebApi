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

public class TemperaturStatisticService : TemperatureQueryBase, ITemperatureStatisticService
{
    private readonly IMongoCollection<Temperature> _temperatureCollection;
    private readonly IMongoCollection<Device> _deviceCollection;

    public TemperaturStatisticService(MongoDBConnectionProvider connectionProvider)
    {
        _temperatureCollection = connectionProvider.GetTemperatureCollection();
        _deviceCollection = connectionProvider.GetDeviceCollection();
    }

    public Chart<NamedSeries> GetStatistic(StatisticRequest request)
    {
        Func<Temperature, bool> predicate = request.Scope.ToTemperaturePredicate();

        var baseResult = GetBaseTemperatureQuery().Where(predicate);

        var max = FilterMax(baseResult);
        var min = FilterMin(baseResult);
        var avg = FilterAverage(baseResult);

        return new Chart<NamedSeries>
        {
            Name = request.Scope.Value,
            Series = new List<NamedSeries>
            {
                CreateNameSeries("min", min),
                CreateNameSeries("average", avg),
                CreateNameSeries("max", max),
            },
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

    private IMongoQueryable<Temperature> GetBaseTemperatureQuery()
    {
        return _temperatureCollection.AsQueryable()
            .Join(_deviceCollection.AsQueryable(),
                p => p.DeviceId,
                o => o.Id,
                (p, o) => new Temperature()
                {
                    Id = p.Id,
                    Value = p.Value,
                    RecordDateTime =
                    p.RecordDateTime,
                    DeviceId = p.DeviceId,
                    Device = o
                });
    }
}
