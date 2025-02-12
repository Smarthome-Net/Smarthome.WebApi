using System;
using System.Globalization;
using MongoDB.Bson.Serialization;

namespace SmartHome.MongoService.BsonCustomSerializers;

public class DatetimeOffsetSerializer : IBsonSerializer<DateTimeOffset>
{
    public Type ValueType => typeof(DateTimeOffset);

    public DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadDateTime();
        return DateTimeOffset.FromUnixTimeMilliseconds(value);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTimeOffset value)
    {
        var rawValue = value.ToUnixTimeMilliseconds();
        context.Writer.WriteDateTime(rawValue);
    }

    public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
    {
        if (value is DateTimeOffset dateTime)
        {
            Serialize(context, args, dateTime);
        }
    }

    object IBsonSerializer.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        return Deserialize(context, args);
    }
}
