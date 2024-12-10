using MongoDB.Driver;
using SmartHome.Common.Extensions;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.MongoService.DbContext;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.MongoService.Services;

public class TemperatureService : ITemperatureService
{
    private readonly MongoDbContext _dbContext;

    public TemperatureService(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateTemperature(Temperature temperature, CancellationToken cancellationToken = default)
    {
        await _dbContext.TemperatureCollection!.InsertOneAsync(temperature, cancellationToken: cancellationToken);
    }

    public IEnumerable<TemperatureDto> GetTemperature(Scope? scope)
    {
        var predicate = scope?.ToDevicePredicate();
        var temperatureQuery = _dbContext.TemperatureCollection.AsQueryable();
        var deviceQuery = _dbContext.DeviceCollection.AsQueryable();

        return deviceQuery
            .Where(predicate!)
            .Join(temperatureQuery,
                device => device.Id,
                temperature => temperature.DeviceId,
                (device, temperature) => temperature.ToDto(device))
            .OrderByDescending(item => item.RecordDateTime);
    }
}
