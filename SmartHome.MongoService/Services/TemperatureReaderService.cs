using MongoDB.Driver;
using SmartHome.Common.Extensions;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.MongoService.DbContext;
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

    public IEnumerable<TemperatureDto> GetTemperature(TemperatureRequest request)
    {
        var predicate = request.Scope?.ToDevicePredicate();
        var temperatureQuery = _dbContext.TemperatureCollection.AsQueryable();
        var deviceQuery = _dbContext.DeviceCollection.AsQueryable();

        return deviceQuery
            .Where(predicate!)
            .Join(temperatureQuery,
                device => device.Id,
                temperature => temperature.DeviceId,
                (device, temperature) => new TemperatureDto()
                {
                    Id = temperature.Id.ToString(),
                    Value = temperature.Value,
                    RecordDateTime = temperature.RecordDateTime,
                    Device = device.ToDto()
                })
            .OrderByDescending(item => item.RecordDateTime);
    }
}
