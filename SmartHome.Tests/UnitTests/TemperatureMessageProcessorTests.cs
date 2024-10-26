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
using SmartHome.Common.Models.Dto;

namespace SmartHome.Tests.UnitTests;

public class TemperatureMessageProcessorTests
{
    private const string DeviceContext = "r/n";
    private const long Timestamp = 1724057532524;
    private readonly ObjectId _deviceId = ObjectId.GenerateNewId();
    private TemperatureMessageProcessor? _messageProcessor;
    
    [SetUp]
    public void Setup() 
    {
        var nullLogger = NullLogger<TemperatureMessageProcessor>.Instance;
        var temperatureWriterMock = new Mock<ITemperatureWriterService>();
        var deviceServiceMock = new Mock<IDeviceService>();

        deviceServiceMock
            .Setup(s => s.GetOrCreateDeviceByTopic(DeviceContext, It.IsAny<CancellationToken>()))
            .Returns(() =>
            {
                const string name = "n";
                const string room = "r";
                const string topic = $"{room}/{name}";
                return Task.FromResult(new Device
                {
                    Id = _deviceId,
                    Name = name,
                    Room = room,
                    Topic = topic,
                });
            });

        _messageProcessor = new TemperatureMessageProcessor(nullLogger, temperatureWriterMock.Object, deviceServiceMock.Object);
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
            PayloadSegment = []
        };

        var action = () => _messageProcessor?.ProcessMessage(message, "");

        action.Should()
            .ThrowAsync<ApplicationMessageException>();
    }

    [Test]
    public async Task TestTemperatureMessageProcessorWithValidMessageReturnsTemperature()
    {
        var rawValue = new { value = 23, time = Timestamp };
        var bytes = JsonSerializer.SerializeToUtf8Bytes(rawValue);
        var message = new MqttApplicationMessageBuilder()
            .WithPayload(bytes)
            .WithTopic("smarthome/sensors/temperature/Badezimmer/Dusche")
            .Build();

        var expected = new TemperatureDto()
        {
            Id = ObjectId.Empty.ToString(),
            RecordDateTime = DateTimeOffset.FromUnixTimeMilliseconds(Timestamp),
            Value = 23f,
            Device = new DeviceDto
            {
                Id = _deviceId.ToString(),
                Topic = DeviceContext,
                Name = "n",
                Room = "r"
            }
        };

        var result = await _messageProcessor?.ProcessMessage(message, DeviceContext)!;
        
        result.Should()
            .NotBeNull()
            .And
            .BeEquivalentTo(expected);
    }

    [Test]
    public void TestTemperatureMessageProcessorObjectIsDisposed() 
    {
        _messageProcessor?.Dispose();

        var message = new MqttApplicationMessage
        {
            PayloadSegment = []
        };

        var action = () => _messageProcessor?.ProcessMessage(message, "");

        action.Should()
            .ThrowAsync<ApplicationMessageException>()
            .WithInnerExceptionExactly(typeof(ObjectDisposedException));
    }
}
