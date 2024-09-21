using FluentAssertions;
using Moq;
using MQTTnet;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.MqttActions;
using SmartHome.MqttService.Observables;
using System.Text.Json;

namespace SmartHome.Tests.UnitTests;

public class TemperatureMqttActionTests
{
    private const string DeviceContext = "r/n";
    private const long Timestamp = 1724057532524;

    private TemperatureMqttAction _temperatureAction;

    [OneTimeSetUp]
    public void OneTimeSetUp() 
    {
        var messageProcessorMock = new Mock<IApplicationMessageProcessor<Temperature>>();
        var _temperatureObservableMock = new Mock<ITemperatureObservable>();
        
        _temperatureAction = new TemperatureMqttAction(messageProcessorMock.Object, _temperatureObservableMock.Object);
    }

    [Test]
    public void TestExcecuteAction()
    {
        var rawValue = new { value = 23, time = Timestamp };
        var bytes = JsonSerializer.SerializeToUtf8Bytes(rawValue);
        var message = new MqttApplicationMessage
        {
            PayloadSegment = new ArraySegment<byte>(bytes),
            Topic = "smarthome/sensors/temperature/Badezimmer/Dusche"
        };

        var action = _temperatureAction.ExecuteAction(message, DeviceContext);
        
        action.Should();
    }
}
