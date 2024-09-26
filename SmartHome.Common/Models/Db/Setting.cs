using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartHome.Common.Models.Db;

[BsonKnownTypes(typeof(PageSetting))]
public class Setting
{
    public ObjectId Id { get; set; }
    public string? Discription { get; set; }
}
