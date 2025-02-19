using System;
using MongoDB.Bson.Serialization;

namespace SmartHome.MongoService.BsonCustomSerializers;

public class SmartHomeSerializerProvider : IBsonSerializationProvider
{
    public IBsonSerializer? GetSerializer(Type type)
    {
        return type == typeof(DateTimeOffset) 
            ? new DatetimeOffsetSerializer() 
            : null;
    }
}