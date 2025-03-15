using System;
using MongoDB.Driver;

namespace SmartHome.MongoService.DbContext;

public interface IMongoDbManagementContext : IDisposable
{
    IMongoClient MongoClient { get; }
    IMongoDatabase Database { get; }
}