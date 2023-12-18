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
using System.Threading.Tasks;

namespace SmartHome.MongoService.Services
{
    public class TemperaturStatisticService : TemperatureQueryBase, ITemperatureStatisticService
    {
        private readonly IMongoCollection<Temperature> _temperatureCollection;
        private readonly IMongoCollection<Device> _deviceCollection;

        public TemperaturStatisticService(MongoDBConnectionProvider connectionProvider)
        {
            _temperatureCollection = connectionProvider.GetTemperatureCollection();
            _deviceCollection = connectionProvider.GetDeviceCollection();
        }

        public IEnumerable<Chart<StatisticSeries>> GetStatistic(Scope scope)
        {
            Func<Temperature, bool> predicate = CreatePredicate(scope.ScopeType, scope.Value);

            return new List<Chart<StatisticSeries>>
            {
                new Chart<StatisticSeries>
                {
                    Name = scope.Value,
                    Series = new List<StatisticSeries>
                    {
                        CreateMaxStatistic(predicate),
                        CreateMinStatistic(predicate),
                        CreateAverageStatistic(predicate)
                    },
                }
            };
        }

        public StatisticSeries CreateMaxStatistic(Func<Temperature, bool> predicate)
        {
            return new StatisticSeries
            {
                Name = "Max",
                Value = QueryMax(predicate),
            };
        }

        private StatisticSeries CreateMinStatistic(Func<Temperature, bool> predicate)
        {
            return new StatisticSeries
            {
                Name = "Min",
                Value = QueryMin(predicate),
            };
        }

        private StatisticSeries CreateAverageStatistic(Func<Temperature, bool> predicate)
        {
            return new StatisticSeries
            {
                Name = "Durchschnitt",
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
}
