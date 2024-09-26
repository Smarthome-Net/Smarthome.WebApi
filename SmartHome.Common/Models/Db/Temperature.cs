using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartHome.Common.Models.Db;

public class Temperature
{
    public ObjectId Id { get; set; }
    public DateTimeOffset RecordDateTime { get; set; }
    public float Value { get; set; }
    public ObjectId DeviceId { get; set; }

    [BsonIgnore]
    public Device? Device { get; set; }
}
