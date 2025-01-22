using MQTTnet;

namespace SmartHome.MqttService;

/// <summary>
/// Factory delegate to get an mqtt factory instance
/// </summary>
/// <returns></returns>
public delegate MqttFactory MqttFactoryProvider();