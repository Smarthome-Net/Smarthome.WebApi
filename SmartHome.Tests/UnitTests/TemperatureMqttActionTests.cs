using FluentAssertions;
using Moq;
using MQTTnet;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.MqttActions;
using SmartHome.MqttService.Observables;
using System.Text.Json;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Tests.UnitTests;

public class TemperatureMqttActionTests
{
    private const string DeviceContext = "r/n";
    private const long Timestamp = 1724057532524;

    private TemperatureMqttAction _temperatureAction;

    [OneTimeSetUp]
    public void OneTimeSetUp() 
    {
        var messageProcessorMock = new Mock<IApplicationMessageProcessor<TemperatureDto>>();
        var temperatureObservableMock = new Mock<ITemperatureObservable>();
        
        _temperatureAction = new TemperatureMqttAction(messageProcessorMock.Object, temperatureObservableMock.Object);
    }

    [Test]
    public void TestExecuteAction()
    {
        var rawValue = new { value = 23, time = Timestamp };
        var bytes = JsonSerializer.SerializeToUtf8Bytes(rawValue);
        var message = new MqttApplicationMessage
        {
            PayloadSegment = new ArraySegment<byte>(bytes),
            Topic = "smarthome/sensors/temperature/Badezimmer/Dusche"
        };

        var action = () => _temperatureAction.ExecuteAction(message, DeviceContext);
        
        action.Should().CompleteWithinAsync(TimeSpan.FromMicroseconds(1));
    }
}
