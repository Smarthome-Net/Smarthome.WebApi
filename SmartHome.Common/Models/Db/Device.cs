using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;

namespace SmartHome.Common.Models.Db;

public class Device
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public string Room { get; set; }
    public string Topic { get; set; }

    [BsonIgnore]
    public DeviceConfiguration Configuration { get; set; }

}

public class DeviceComparer : IEqualityComparer<Device>
{
    public bool Equals(Device x, Device y)
    {
        return string.Equals(x.Room, y.Room);
    }

    public int GetHashCode([DisallowNull] Device obj)
    {
        return obj.Room.GetHashCode();
    }
}
