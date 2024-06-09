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
        var predicate = request.Scope.ToDevicePredicate();
        var temperatureQuery = _dbContext.TemperatureCollection.AsQueryable();
        var deviceQuery = _dbContext.DeviceCollection.AsQueryable();

        return deviceQuery
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
                })
            .ToStatisticChart(request.Scope);
    }
}
