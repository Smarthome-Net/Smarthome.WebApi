using MongoDB.Driver;
using SmartHome.Common.Extensions;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto;
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
        var predicate = request.Scope?.ToDevicePredicate();
        var temperatureQuery = _dbContext.TemperatureCollection.AsQueryable();
        var deviceQuery = _dbContext.DeviceCollection.AsQueryable();

        return deviceQuery
            .Where(predicate!)
            .Join(temperatureQuery.AsQueryable(),
                device => device.Id,
                temperature => temperature.DeviceId,
                (device, temperature) => new TemperatureDto()
                {
                    Id = temperature.Id.ToString(),
                    Value = temperature.Value,
                    RecordDateTime = temperature.RecordDateTime,
                    Device = device.ToDto(),
                })
            .ToStatisticChart(request.Scope!);
    }
}
