using MongoDB.Bson;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using System.Collections.Generic;
using System.Linq;

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

    public static IEnumerable<DeviceDto> ToDto(this IEnumerable<Device> devices)
    {
        return devices.Select(ToDto);
    }

    public static Device? ToDb(this DeviceDto device) 
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
