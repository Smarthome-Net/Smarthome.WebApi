using MQTTnet;
using SmartHome.Common.Models.Dto;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.Observables;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SmartHome.MqttService.MqttActions;

public class TemperatureMqttAction : IMqttAction
{
    private readonly ITemperatureObservable _temperatureObservable;
    private readonly IApplicationMessageProcessor<TemperatureDto> _messageProcessor;
    private readonly ILogger<TemperatureMqttAction> _logger;

    public TemperatureMqttAction(IApplicationMessageProcessor<TemperatureDto> messageProcessor, ITemperatureObservable temperatureObservable, ILogger<TemperatureMqttAction> logger)
    {
        _messageProcessor = messageProcessor;
        _temperatureObservable = temperatureObservable;
        _logger = logger;
    }

    public async Task ExecuteAction(MqttApplicationMessage message, string deviceContext, CancellationToken token = default)
    {
        var temperature = await _messageProcessor.ProcessMessage(message, deviceContext, token);
        _temperatureObservable.OnNext(temperature);
        _logger.LogInformation("Temperature Mqtt Action executed");
    }

    public string GetSensorType() => SensorType.Temperature;
}
