using MongoDB.Bson;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Extensions.Mapping;

public static class DeviceMapping
{
    public static DeviceDto ToDto(this Device device)
    {
        return new DeviceDto 
        { 
            Id = device.Id.ToString(),
            Name = device.Name,
            Room = device.Room,
            Topic = device.Topic,
        };
    }

    public static Device? ToDB(this DeviceDto device) 
    {
        if(!ObjectId.TryParse(device.Id, out var id))
        {
            return null;
        }
        
        return new Device
        {
            Id = id,
            Name = device.Name,
            Room = device.Room,
            Topic = device.Topic,
        };
    }
}
