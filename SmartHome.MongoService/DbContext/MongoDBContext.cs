using MongoDB.Driver;
using System;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.Settings;

namespace SmartHome.MongoService.DbContext;

public class MongoDBContext
{
    private readonly MongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    public MongoDBContext(DbConnectionSetting dbConnection)
    {
        try
        {
            _mongoClient = new MongoClient(dbConnection.GetMongoConnectionString());
            _database = _mongoClient.GetDatabase(dbConnection.Database);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to connect with database server: {ex.Message}");
        }
    }

    public IMongoCollection<Device> DeviceCollection => _database.GetCollection<Device>(Collection.Device);

    public IMongoCollection<Temperature> TemperatureCollection => _database.GetCollection<Temperature>(Collection.Temperature);
}
