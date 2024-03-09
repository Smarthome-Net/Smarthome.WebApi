using SmartHome.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.MongoService.Provider;
using MongoDB.Driver;
using System;
using SmartHome.Common.Models.Db;
using System.Threading;

namespace SmartHome.MongoService.Services;

public class DeviceService : IDeviceService
{
    private readonly IMongoCollection<Device> _deviceCollection;
    public DeviceService(MongoDBConnectionProvider connectionProvider) 
    {
        _deviceCollection = connectionProvider.GetDeviceCollection();
    }

    public async Task<Device> GetDeviceByTopic(string topic, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Topic, topic);
        var result = await _deviceCollection.FindAsync(filter, cancellationToken: cancellationToken);
        return result.FirstOrDefault(cancellationToken);
    }

    public async Task<Device> GetDeviceById(string deviceId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(d => d.Id, deviceId);
        var result = await _deviceCollection.FindAsync(filter, cancellationToken: cancellationToken);
        return result.FirstOrDefault(cancellationToken);
    }

    public async Task<IEnumerable<Device>> GetDevices(string room, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Room, room);
        var result = await _deviceCollection.FindAsync(filter, cancellationToken: cancellationToken);
        return result.ToEnumerable(cancellationToken);
    }

    public async Task<IEnumerable<Device>> GetAllDevices(CancellationToken cancellationToken = default)
    {
        var result = await _deviceCollection.FindAsync(device => true, cancellationToken: cancellationToken);
        return result.ToEnumerable(cancellationToken);
    }

    public async Task<long> UpdateDevice(Device device, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Id, device.Id);
        var update = Builders<Device>.Update
            .Set(p => p.Name, device.Name)
            .Set(p => p.Room, device.Room)
            .Set(p => p.Topic, device.Topic);

        var result = await _deviceCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if(result.IsAcknowledged) 
        {
            return result.ModifiedCount;
        }
        return 0;
    }
    public async Task<Device> CreateDevice(Device device, CancellationToken cancellationToken = default)
    {
        await _deviceCollection.InsertOneAsync(device, cancellationToken: cancellationToken);
        return device;
    }

    public async Task<long> DeleteDevice(string deviceId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Id, deviceId);
        var result = await _deviceCollection.DeleteOneAsync(filter, cancellationToken);
        
        if(result.IsAcknowledged) 
        {
            return result.DeletedCount;
        }
        return 0;
    }

    public async Task<Device> GetOrCreateDeviceByTopic(string topic, CancellationToken cancellationToken = default)
    {
        const string Separator = "/";
        if (!topic.Contains(Separator)) {
            throw new ArgumentException($"Argument does not contain {Separator}, the argument was: {topic}");
        }

        var device = await GetDeviceByTopic(topic, cancellationToken);
        if (device is null) {
            var splited = topic.Split(Separator);
            device = new Device()
            {
                Room = splited[0],
                Name = splited[1],
                Topic = topic
            };
            await CreateDevice(device, cancellationToken);
        }
        return device;
    }
}
