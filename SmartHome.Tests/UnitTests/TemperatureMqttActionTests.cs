using FluentAssertions;
using Moq;
using MQTTnet;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.MqttActions;
using SmartHome.MqttService.Observables;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SmartHome.Common.Models.Dto;
using SmartHome.MqttService.Extensions;
using SmartHome.MqttService.Settings;

namespace SmartHome.Tests.UnitTests;

public class TemperatureMqttActionTests
{
    private const long Timestamp = 1724057532524;

    private TemperatureMqttAction _temperatureAction;

    [OneTimeSetUp]
    public void OneTimeSetUp() 
    {
        var messageProcessorMock = new Mock<IApplicationMessageProcessor<TemperatureDto>>();
        var temperatureObservableMock = new Mock<ITemperatureObservable>();
        var logger = NullLogger<TemperatureMqttAction>.Instance;
        var options = new Mock<IOptions<MqttOptions>>();

        options.Setup(s => s.Value).Returns(new MqttOptions
        {
            MqttSetting = new MqttSetting
            {
                BrokerSetting = new BrokerSetting
                {
                    Host = string.Empty
                },
                ClientSetting = new ClientSetting
                {
                    Id = string.Empty,
                    UserName = string.Empty,
                    Password = string.Empty,
                },
                TopicSetting = new TopicSetting
                {
                    SubscriptionTopic = "smarthome/sensors/temperature",
                    SubscriptionRpcTopic = string.Empty
                }
            }
        });
        
        _temperatureAction = new TemperatureMqttAction(messageProcessorMock.Object, temperatureObservableMock.Object, logger, options.Object);
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

        var action = () => _temperatureAction.ProcessAction(message);
        
        action.Should().CompleteWithinAsync(TimeSpan.FromMicroseconds(1));
    }
}
