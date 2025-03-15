using System;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.DbContext;

public class MongoDbManagementContext : IMongoDbManagementContext
{
    public IMongoClient MongoClient { get; }

    public IMongoDatabase Database { get; }

    private readonly ILogger<MongoDbManagementContext> _logger;

    public MongoDbManagementContext(IMongoClient mongoClient,
                            string database,
                            ILogger<MongoDbManagementContext> logger)
    {
        MongoClient = mongoClient;
        Database = MongoClient.GetDatabase(database);
        _logger = logger;
        _logger.LogInformation("Database context created");
    }

    private void Dispose(bool disposing)
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

    ~MongoDbManagementContext()
    {
        Dispose(false);
    }
}
