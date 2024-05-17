using System.Threading;
using System.Threading.Tasks;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;

namespace SmartHome.MongoService.Services;

public class TemperatureWriterService : ITemperatureWriterService
{
    private readonly MongoDBContext _dbContext;
    public TemperatureWriterService(MongoDBContext dBContext) 
    {
        _dbContext = dBContext;
    }
    public async Task<Temperature> WriteTemperature(Temperature temperature, CancellationToken cancellationToken)
    {
        await _dbContext.TemperatureCollection.InsertOneAsync(temperature, cancellationToken: cancellationToken);
        return temperature;
    }
}
