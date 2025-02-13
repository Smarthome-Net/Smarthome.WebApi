using System;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.DbContext;

public class MongDBManagementContext : IDisposable
{
    public IMongoClient MongoClient { get; }

    public IMongoDatabase Database { get; }

    private readonly ILogger<MongDBManagementContext> _logger;

    public MongDBManagementContext(IMongoClient mongoClient,
                            string database,
                            ILogger<MongDBManagementContext> logger)
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

    ~MongDBManagementContext()
    {
        Dispose(false);
    }
}
