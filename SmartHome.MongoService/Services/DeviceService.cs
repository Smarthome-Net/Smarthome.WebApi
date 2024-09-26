using SmartHome.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartHome.MongoService.DbContext;
using MongoDB.Driver;
using System;
using SmartHome.Common.Models.Db;
using System.Threading;
using SmartHome.Common.Collections;
using MongoDB.Bson;

namespace SmartHome.MongoService.Services;

public class DeviceService : IDeviceService
{
    private readonly MongoDBContext _dbContext;
    public DeviceService(MongoDBContext dbContext) 
    {
        _dbContext = dbContext;
    }

    public async Task<Device> GetDeviceByTopic(string topic, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Topic, topic);
        var result = await _dbContext.DeviceCollection.FindAsync(filter, cancellationToken: cancellationToken);
        return result.FirstOrDefault(cancellationToken);
    }

    public async Task<Device> GetDeviceById(string deviceId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(d => d.Id, ObjectId.Parse(deviceId));
        var result = await _dbContext.DeviceCollection.FindAsync(filter, cancellationToken: cancellationToken);
        return result.FirstOrDefault(cancellationToken);
    }

    public async Task<IEnumerable<Device>> GetDevices(string room, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Room, room);
        var result = await _dbContext.DeviceCollection.FindAsync(filter, cancellationToken: cancellationToken);
        return result.ToEnumerable(cancellationToken);
    }

    public async Task<IEnumerable<Device>> GetAllDevices(CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.DeviceCollection.FindAsync(device => true, cancellationToken: cancellationToken);
        return result.ToEnumerable(cancellationToken);
    }

    public async Task<long> UpdateDevice(Device device, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Id, device.Id);
        var update = Builders<Device>.Update
            .Set(p => p.Name, device.Name)
            .Set(p => p.Room, device.Room)
            .Set(p => p.Topic, device.Topic);

        var result = await _dbContext.DeviceCollection!.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        if(result.IsAcknowledged) 
        {
            return result.ModifiedCount;
        }
        return 0;
    }
    public async Task<Device> CreateDevice(Device device, CancellationToken cancellationToken = default)
    {
        await _dbContext.DeviceCollection!.InsertOneAsync(device, cancellationToken: cancellationToken);
        return device;
    }

    public async Task<long> DeleteDevice(string deviceId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Device>.Filter.Eq(p => p.Id, ObjectId.Parse(deviceId));
        var result = await _dbContext.DeviceCollection!.DeleteOneAsync(filter, cancellationToken);
        
        if(result.IsAcknowledged) 
        {
            return result.DeletedCount;
        }
        return 0;
    }

    public async Task<Device> GetOrCreateDeviceByTopic(string topic, CancellationToken cancellationToken = default)
    {
        var segments = Segments.FromString(topic);
        if (segments.Count == 1) {
            throw new ArgumentException($"Topic has not enough segments: {segments.Count} segments");
        }

        var device = await GetDeviceByTopic(topic, cancellationToken);
        if (device is null) {
            device = new Device()
            {
                Room = segments[0].Value,
                Name = segments[1].Value,
                Topic = topic
            };
            await CreateDevice(device, cancellationToken);
        }
        return device;
    }
}
