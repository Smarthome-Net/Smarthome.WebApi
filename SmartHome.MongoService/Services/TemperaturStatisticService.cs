using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Extensions;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.DTO;
using SmartHome.Common.Models.DTO.Charts;
using SmartHome.Common.Models.DTO.Requests;
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

    public IEnumerable<Chart<NamedSeries>> GetStatistic(StatisticRequest request)
    {
        Func<Temperature, bool> predicate = request.Scope.ToTemperaturePredicate();

        return new List<Chart<NamedSeries>>
        {
            new Chart<NamedSeries>
            {
                Name = request.Scope.Value,
                Series = new List<NamedSeries>
                {
                    CreateMaxStatistic(predicate),
                    CreateMinStatistic(predicate),
                    CreateAverageStatistic(predicate)
                },
            }
        };
    }

    public NamedSeries CreateMaxStatistic(Func<Temperature, bool> predicate)
    {
        return new NamedSeries
        {
            Name = "max",
            Value = QueryMax(predicate),
        };
    }

    private NamedSeries CreateMinStatistic(Func<Temperature, bool> predicate)
    {
        return new NamedSeries
        {
            Name = "min",
            Value = QueryMin(predicate),
        };
    }

    private NamedSeries CreateAverageStatistic(Func<Temperature, bool> predicate)
    {
        return new NamedSeries
        {
            Name = "average",
            Value = QueryAverage(predicate),
        };
    }

    private float QueryMax(Func<Temperature, bool> predicate)
    {
        return GetBaseTemperatureQuery()
            .Where(predicate)
            .OrderByDescending(item => item.Value)
            .Select(item => item.Value)
            .FirstOrDefault();
    }

    private float QueryMin(Func<Temperature, bool> predicate)
    {
        return GetBaseTemperatureQuery()
            .Where(predicate)
            .OrderBy(item => item.Value)
            .Select(item => item.Value)
            .FirstOrDefault();
    }

    private float QueryAverage(Func<Temperature, bool> predicate)
    {
        return GetBaseTemperatureQuery()
            .Where(predicate)
            .Average(i => i.Value);
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
