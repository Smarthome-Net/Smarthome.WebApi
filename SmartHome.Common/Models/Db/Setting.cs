using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartHome.Common.Models.Db;

[BsonKnownTypes(typeof(PageSetting))]
[BsonKnownTypes(typeof(CommonSetting))]
public class Setting
{
    public ObjectId Id { get; set; }
    public string? Description { get; set; }
}
