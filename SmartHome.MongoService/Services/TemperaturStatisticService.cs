using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Extensions;
using SmartHome.Common.Helpers;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.MongoService.DbContext;
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
        var predicate = request.Scope.ToPredicate();
        var temperatureQuery = _dbContext.TemperatureCollection.AsQueryable();
        var deviceQuery = _dbContext.DeviceCollection.AsQueryable();

        var result = deviceQuery
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

        var max = result.Max(x => x.Value);
        var min = result.Min(x => x.Value);
        var avg = result.Average(x => x.Value);

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
}
