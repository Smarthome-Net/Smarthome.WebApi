using System;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.DbContext;

public class MongDbManagementContext : IDisposable
{
    public IMongoClient MongoClient { get; }

    public IMongoDatabase Database { get; }

    private readonly ILogger<MongDbManagementContext> _logger;

    public MongDbManagementContext(IMongoClient mongoClient,
                            string database,
                            ILogger<MongDbManagementContext> logger)
    {
        MongoClient = mongoClient;
        Database = MongoClient.GetDatabase(database);
        _logger = logger;
        _logger.LogInformation("Database context created");
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            MongoClient.Dispose();
        }
    }

    public void Dispose()
    {
        _logger.LogInformation("Disposing MongoDBContext");
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~MongDbManagementContext()
    {
        Dispose(false);
    }
}
