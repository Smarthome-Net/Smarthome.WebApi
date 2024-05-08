using MQTTnet;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.Observables;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.MqttService.MqttActions;

public class TemperatureMqttAction : IMqttAction
{
    private readonly ITemperatureObservable _temperatureObservable;
    private readonly IApplicationMessageProcessor<Temperature> _messageProcessor;

    public TemperatureMqttAction(IApplicationMessageProcessor<Temperature> messageProcessor, ITemperatureObservable temperatureObservable)
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
