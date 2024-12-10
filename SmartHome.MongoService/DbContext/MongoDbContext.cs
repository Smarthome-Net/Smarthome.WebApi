using System;
using MongoDB.Driver;
using SmartHome.Common.Models.Db;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.DbContext;

public class MongoDbContext : IDisposable
{
    private readonly IMongoClient _mongoClient;
    private readonly ILogger<MongoDbContext> _logger;
    private readonly IMongoDatabase _database;
    
    public MongoDbContext(IMongoClient mongoClient, 
                            string database, 
                            ILogger<MongoDbContext> logger)
    {
        _mongoClient = mongoClient;
        _logger = logger;
        _database = _mongoClient.GetDatabase(database);
        _logger.LogInformation("Database context created");
    }

    public IMongoCollection<Device> DeviceCollection => _database.GetCollection<Device>(Collection.Device);

    public IMongoCollection<Temperature> TemperatureCollection => _database.GetCollection<Temperature>(Collection.Temperature);
    
    public IMongoCollection<Setting> SettingCollection => _database.GetCollection<Setting>(Collection.Setting);

    
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _mongoClient.Dispose();
        }
    }

    public void Dispose()
    {
        _logger.LogInformation("Disposing MongoDBContext");
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~MongoDbContext()
    {
        Dispose(false);
    }
}
