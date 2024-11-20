using MongoDB.Driver;
using SmartHome.Common.Models.Db;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.DbContext;

public class MongoDBContext
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    
    public MongoDBContext(IMongoClient mongoClient, 
                            string database, 
                            ILogger<MongoDBContext> logger)
    {
        _mongoClient = mongoClient;
        _database = _mongoClient.GetDatabase(database);
        logger.LogInformation("Database context created");
    }

    public IMongoCollection<Device> DeviceCollection => _database.GetCollection<Device>(Collection.Device);

    public IMongoCollection<Temperature> TemperatureCollection => _database.GetCollection<Temperature>(Collection.Temperature);
    
    public IMongoCollection<Setting> SettingCollection => _database.GetCollection<Setting>(Collection.Setting);
}
