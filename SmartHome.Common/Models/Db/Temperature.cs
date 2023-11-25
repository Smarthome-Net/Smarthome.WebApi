using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SmartHome.Common.Models.Db;

public class Temperature
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public DateTimeOffset RecordDateTime { get; set; }
    public float Value { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string DeviceId { get; set; }

    [BsonIgnore]
    public Device Device { get; set; }
}
