using SmartHome.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.MongoService.Provider;
using MongoDB.Driver;
using System;
using SmartHome.Common.Models.Db;
using MongoDB.Bson;

namespace SmartHome.MongoService.Services;

public class DeviceService : IDeviceService
{
    private readonly IMongoCollection<Device> _deviceCollection;
    public DeviceService(MongoDBConnectionProvider connectionProvider) 
    {
        _deviceCollection = connectionProvider.GetDeviceCollection();
    }

    public async Task<Device> GetDeviceByTopic(string topic)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Topic, topic);
        var result = await _deviceCollection.FindAsync(filter);
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<Device>> GetDevices(string room)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Room, room);
        var result = await _deviceCollection.FindAsync(filter);
        return result.ToEnumerable();
    }

    public async Task<IEnumerable<Device>> GetAllDevices()
    {
        var result = await _deviceCollection.FindAsync(device => true);
        return result.ToEnumerable();
    }

    public async Task<IEnumerable<Device>> GetRooms()
    {
        var result = await _deviceCollection.FindAsync(device => true);
        return result.ToEnumerable().Distinct(new DeviceComparer());
    }

    public async Task<long> UpdateDevice(Device device)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Id, device.Id);
        var update = Builders<Device>.Update
            .Set(p => p.Name, device.Name)
            .Set(p => p.Room, device.Room)
            .Set(p => p.Topic, device.Topic);

        var result = await _deviceCollection.UpdateOneAsync(filter, update);
        if(result.IsAcknowledged) 
        {
            return result.ModifiedCount;
        }
        return 0;
    }
    public async Task<Device> CreateDevice(Device device)
    {
        await _deviceCollection.InsertOneAsync(device);
        return device;
    }

    public async Task<long> DeleteDevice(string deviceId)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Id, deviceId);
        var result = await _deviceCollection.DeleteOneAsync(filter);
        
        if(result.IsAcknowledged) 
        {
            return result.DeletedCount;
        }
        return 0;
    }

    public async Task<Device> GetOrCreateDeviceByTopic(string topic)
    {
        const string Separator = "/";
        if (!topic.Contains(Separator)) {
            throw new ArgumentException($"Argument does not contain {Separator}, the argument was: {topic}");
        }

        var device = await GetDeviceByTopic(topic);
        if (device is null) {
            var splited = topic.Split(Separator);
            device = new Device()
            {
                Room = splited[0],
                Name = splited[1],
                Topic = topic
            };
            await CreateDevice(device);
        }
        return device;
    }

    
}
