using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using MongoDB.Bson;
using Moq;
using MQTTnet;
using SmartHome.Common.Exceptions;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;
using System.Text.Json;

namespace SmartHome.Tests.UnitTests;

public class TemperatureMessageProcessorTests
{
    private const string DeviceContext = "r/n";
    private const long Timestamp = 1724057532524;
    private readonly ObjectId deviceId = ObjectId.GenerateNewId();
    private TemperatureMessageProcessor? _messageProcessor;
    
    [SetUp]
    public void Setup() 
    {
        var nullLoger = NullLogger<TemperatureMessageProcessor>.Instance;
        var temperatureWriterMock = new Mock<ITemperatureWriterService>();
        var deviceServiceMock = new Mock<IDeviceService>();

        deviceServiceMock
            .Setup(s => s.GetOrCreateDeviceByTopic(DeviceContext, It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                var name = "n";
                var room = "r";
                var topic = $"{room}/{name}";
                return Task.FromResult(new Device
                {
                    Id = deviceId,
                    Name = name,
                    Room = room,
                    Topic = topic,
                });
            });

        _messageProcessor = new TemperatureMessageProcessor(nullLoger, temperatureWriterMock.Object, deviceServiceMock.Object);
    }

    [TearDown] 
    public void TearDown()
    {
        _messageProcessor?.Dispose();
    }

    [Test]
    public void TestTemperatureMessageProcessorWithEmptyMessageShouldThrow() 
    {
        var message = new MqttApplicationMessage
        {
            PayloadSegment = new ArraySegment<byte>()
        };

        var action = () => _messageProcessor?.ProcessMessage(message, "");

        action.Should()
            .ThrowAsync<ApplicationMessageException>();
    }

    [Test]
    public async Task TestTemperateMessageProcessorWithValidMessageReturnsTemperature()
    {
        var rawValue = new { value = 23, time = Timestamp };
        var bytes = JsonSerializer.SerializeToUtf8Bytes(rawValue);
        var message = new MqttApplicationMessageBuilder()
            .WithPayload(bytes)
            .WithTopic("smarthome/sensors/temperature/Badezimmer/Dusche")
            .Build();

        var expected = new Temperature
        {
            RecordDateTime = DateTimeOffset.FromUnixTimeMilliseconds(Timestamp),
            Value = 23f,
            DeviceId = deviceId,
        };

        var result = await _messageProcessor?.ProcessMessage(message, DeviceContext)!;
        
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expected, options => options
            .Excluding(t => t.Device)
            .Excluding(t => t.Id));
    }

    [Test]
    public void TestTempetartureMessageProcessorObjectIsDisposed() 
    {
        _messageProcessor?.Dispose();

        var message = new MqttApplicationMessage
        {
            PayloadSegment = new ArraySegment<byte>()
        };

        var action = () => _messageProcessor?.ProcessMessage(message, "");

        action.Should()
            .ThrowAsync<ApplicationMessageException>()
            .WithInnerExceptionExactly(typeof(ObjectDisposedException));
    }
}
