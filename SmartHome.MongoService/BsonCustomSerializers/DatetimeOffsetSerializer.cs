using System;
using MongoDB.Bson.Serialization;

namespace SmartHome.MongoService.BsonCustomSerializers;

/// <summary>
/// Custom Date time offset serializer for time series
/// </summary>
public class DatetimeOffsetSerializer : IBsonSerializer<DateTimeOffset>
{
    public Type ValueType => typeof(DateTimeOffset);

    /// <summary>
    /// Deserialization only UTC dates, to prevent any logical error when handling with dates, the Client should format the date
    /// </summary>
    /// <param name="context"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public DateTimeOffset Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var value = context.Reader.ReadDateTime();
        return DateTimeOffset.FromUnixTimeMilliseconds(value);
    }

    /// <summary>
    /// Serialization into datetime with unix milliseconds. Note: The date will be transformed from local into UTC date
    /// </summary>
    /// <param name="context"></param>
    /// <param name="args"></param>
    /// <param name="value"></param>
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
