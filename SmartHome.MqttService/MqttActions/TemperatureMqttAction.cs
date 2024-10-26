using MQTTnet;
using SmartHome.Common.Models.Dto;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.Observables;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.MqttService.MqttActions;

public class TemperatureMqttAction : IMqttAction
{
    private readonly ITemperatureObservable _temperatureObservable;
    private readonly IApplicationMessageProcessor<TemperatureDto> _messageProcessor;

    public TemperatureMqttAction(IApplicationMessageProcessor<TemperatureDto> messageProcessor, ITemperatureObservable temperatureObservable)
    {
        _messageProcessor = messageProcessor;
        _temperatureObservable = temperatureObservable;
    }

    public async Task ExecuteAction(MqttApplicationMessage message, string deviceContext, CancellationToken token = default)
    {
        var temperature = await _messageProcessor.ProcessMessage(message, deviceContext, token);
        _temperatureObservable.OnNext(temperature);
    }

    public string GetSensorType() => SensorType.Temperature;
}
