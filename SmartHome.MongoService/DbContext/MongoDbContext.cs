using System;
using MongoDB.Driver;
using SmartHome.Common.Models.Db;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.DbContext;

public class MongoDbContext : IDisposable
{
    private readonly MongDBManagementContext _managementContext;
    private readonly ILogger<MongoDbContext> _logger;

    public MongoDbContext(MongDBManagementContext managementContext, ILogger<MongoDbContext> logger)
    {
        _managementContext = managementContext;
        _logger = logger;
        _logger.LogInformation("Database context created");
    }

    public IMongoCollection<Device> DeviceCollection => _managementContext.Database.GetCollection<Device>(Collection.Device);

    public IMongoCollection<Temperature> TemperatureCollection => _managementContext.Database.GetCollection<Temperature>(Collection.Temperature);

    public IMongoCollection<Setting> SettingCollection => _managementContext.Database.GetCollection<Setting>(Collection.Setting);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _managementContext.Dispose();
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
