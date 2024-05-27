using MongoDB.Driver;
using System;
using SmartHome.Common.Models.Db;

namespace SmartHome.MongoService.DbContext;

public class MongoDBContext
{
    private readonly IMongoClient _mongoClient;
    private readonly IMongoDatabase _database;
    public MongoDBContext(IMongoClient mongoClient, string database)
    {
        try
        {
            _mongoClient = mongoClient;
            _database = _mongoClient.GetDatabase(database);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to connect with database server: {ex.Message}");
        }
    }

    public IMongoCollection<Device> DeviceCollection => _database.GetCollection<Device>(Collection.Device);

    public IMongoCollection<Temperature> TemperatureCollection => _database.GetCollection<Temperature>(Collection.Temperature);
}
