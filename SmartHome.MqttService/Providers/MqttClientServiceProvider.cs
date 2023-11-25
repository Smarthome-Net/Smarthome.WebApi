using SmartHome.MqttService.Services;

namespace SmartHome.MqttService.Providers;

public class MqttClientServiceProvider
{
    public IMqttClientService MqttClientService { get; }

    public MqttClientServiceProvider(IMqttClientService mqttClientService)
    {
        MqttClientService = mqttClientService;
    }
}
