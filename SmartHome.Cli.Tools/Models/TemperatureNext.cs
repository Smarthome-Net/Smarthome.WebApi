using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SmartHome.MongoService.BsonCustomSerializers;

namespace SmartHome.Cli.Tools.Models;

class TemperatureNext
{
    public ObjectId Id { get; set; }

    [BsonSerializer(typeof(DatetimeOffsetSerializer))]
    public DateTimeOffset RecordDateTime { get; set; }
    public float Value { get; set; }
    public ObjectId DeviceId { get; set; }
}