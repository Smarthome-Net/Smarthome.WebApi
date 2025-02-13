using System;
using MongoDB.Bson.Serialization;

namespace SmartHome.MongoService.BsonCustomSerializers;

public class MongoSerializerBuilder
{
    public MongoSerializerBuilder AddBsonSerializationProvider(IBsonSerializationProvider provider)
    {
        BsonSerializer.RegisterSerializationProvider(provider);
        return this;
    }
}