using MQTTnet;

namespace SmartHome.MqttService;

public interface IMqttFactoryProvider
{
    MqttFactory MqttFactory { get; }
}