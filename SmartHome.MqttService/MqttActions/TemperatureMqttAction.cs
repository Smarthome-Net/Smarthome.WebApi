using SmartHome.Common.Models.Dto;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.Observables;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using Microsoft.Extensions.Options;
using SmartHome.MqttService.Settings;
using SmartHome.MqttService.Extensions;

namespace SmartHome.MqttService.MqttActions;

public class TemperatureMqttAction : IMqttAction
{
    private readonly ITemperatureObservable _temperatureObservable;
    private readonly IApplicationMessageProcessor<TemperatureDto> _messageProcessor;
    private readonly ILogger<TemperatureMqttAction> _logger;
    private readonly TopicSetting _topicSetting;

    public TemperatureMqttAction(IApplicationMessageProcessor<TemperatureDto> messageProcessor, 
        ITemperatureObservable temperatureObservable, 
        ILogger<TemperatureMqttAction> logger, 
        IOptions<MqttOptions> mqttOptions)
    {
        _messageProcessor = messageProcessor;
        _temperatureObservable = temperatureObservable;
        _logger = logger;
        _topicSetting = mqttOptions.Value.MqttSetting.TopicSetting;
    }

    public async Task ProcessAction(MqttApplicationMessage mqttApplicationMessage, CancellationToken token = default)
    {
        var deviceContext = mqttApplicationMessage.GetDeviceContext($"{_topicSetting.SubscriptionTopic}/{SensorType}");
        var temperature = await _messageProcessor.ProcessMessage(mqttApplicationMessage, deviceContext, token);
        _temperatureObservable.OnNext(temperature);
        _logger.LogInformation("Temperature Mqtt Action executed");
    }

    public string SensorType => SensorTypes.Temperature;
}
