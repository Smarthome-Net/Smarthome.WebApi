using MongoDB.Driver;
using System;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.Settings;

namespace SmartHome.MongoService.Provider;

public class MongoDBConnectionProvider
{
    public MongoDBConnectionProvider(DbConnectionSetting dbConnection)
    {
        try
        {
            Client = new MongoClient(dbConnection.GetMongoConnectionString());
            Database = Client.GetDatabase(dbConnection.Database);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to connect with database server: {ex.Message}");
        }
        
    }

    public IMongoClient Client { get; }

    public IMongoDatabase Database { get; }

    public IMongoCollection<Device> GetDeviceCollection()
    {
        return Database.GetCollection<Device>(Collection.Device);
    }

    public IMongoCollection<Temperature> GetTemperatureCollection() 
    {
        return Database.GetCollection<Temperature>(Collection.Temperature);
    }
}
