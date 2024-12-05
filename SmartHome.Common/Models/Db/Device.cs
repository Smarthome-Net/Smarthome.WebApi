using MongoDB.Bson;

namespace SmartHome.Common.Models.Db;

public class Device
{
    public ObjectId Id { get; set; }
    public string? Name { get; set; }
    public string? Room { get; set; }
    public string? Topic { get; set; }
}
