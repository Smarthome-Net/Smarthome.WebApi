using MongoDB.Driver;
using System;
using SmartHome.Common.Models.Db;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.DbContext;

public class MongoDBContext
{
    private readonly IMongoClient _mongoClient;
    private readonly ILogger<MongoDBContext> _logger;
    private IMongoDatabase? _database;
    public MongoDBContext(IMongoClient mongoClient, 
                            ILogger<MongoDBContext> logger)
    {

        _mongoClient = mongoClient;
        _logger = logger;
    }

    internal void ConfigureDatabase(string database) 
    {
        try
        {
            _database = _mongoClient.GetDatabase(database);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unable to connect with database server: {Message}", ex.Message);
        }
    }

    public IMongoCollection<Device>? DeviceCollection => _database!.GetCollection<Device>(Collection.Device);

    public IMongoCollection<Temperature>? TemperatureCollection => _database!.GetCollection<Temperature>(Collection.Temperature);
    
    public IMongoCollection<Setting>? SettingCollection => _database!.GetCollection<Setting>(Collection.Setting);
}
