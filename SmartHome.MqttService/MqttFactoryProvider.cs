using MQTTnet;

namespace SmartHome.MqttService;

public class MqttFactoryProvider : IMqttFactoryProvider
{
    public MqttFactory MqttFactory => new MqttFactory();
}