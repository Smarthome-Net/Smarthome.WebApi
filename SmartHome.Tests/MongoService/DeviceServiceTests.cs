using FluentAssertions;
using MongoDB.Driver;
using System.Linq;
using Moq;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;
using SmartHome.MongoService.Services;
using ZstdSharp.Unsafe;

namespace SmartHome.Tests;

public class DeviceServiceTests
{
    private DeviceService? _deviceService;
    private Mock<IMongoCollection<Device>> _collectionMock;

    [SetUp]
    public void SetUp() 
    {
        
        var mongoClientMock = new Mock<IMongoClient>();
        var mongoDatabaseMock = new Mock<IMongoDatabase>();
        _collectionMock = new Mock<IMongoCollection<Device>>();
        
        
        mongoDatabaseMock
            .Setup(c => c.GetCollection<Device>(It.IsAny<string>(), null))
            .Returns(_collectionMock.Object);
        
        mongoClientMock
            .Setup(d => d.GetDatabase("test", null))
            .Returns(mongoDatabaseMock.Object);
        
        var mongoContext = new MongoDBContext(mongoClientMock.Object, "test");
        _deviceService = new DeviceService(mongoContext);
    }

    [Test]
    public async Task TestGetDeviceByTopicReturnsValidDevice() 
    {
        IAsyncCursor<Device> deviceCurser = new DeviceTestCurser();
        var filter = Builders<Device>.Filter.Eq(d => d.Topic, "test/device");
        _collectionMock
            .Setup(c => c.FindAsync<Device>(filter, null, It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(deviceCurser));
        var expected = new Device
        {
            Topic = "test/device"
        };
        var result = await _deviceService!.GetDeviceByTopic("test/device");

        result.Should()
            .BeEquivalentTo(expected, op => op
                .ExcludingProperties()
                .Including(p => p.Topic));
    }
}