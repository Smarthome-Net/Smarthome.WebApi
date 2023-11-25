using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.Provider;

namespace SmartHome.MongoService.Services;

public class TemperatureWriterService : ITemperatureWriterService
{
    private readonly IMongoCollection<Temperature> _temperatureCollection;
    public TemperatureWriterService(MongoDBConnectionProvider connectionProvider) 
    {
        _temperatureCollection = connectionProvider.GetTemperatureCollection();
    }
    public async Task<Temperature> WriteTemperature(Temperature temperature, CancellationToken cancellationToken)
    {
        await _temperatureCollection.InsertOneAsync(temperature, cancellationToken: cancellationToken);
        return temperature;
    }
}
