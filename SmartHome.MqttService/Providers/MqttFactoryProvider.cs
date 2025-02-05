using MQTTnet;

namespace SmartHome.MqttService.Providers;

/// <summary>
/// Factory delegate to get a mqtt factory instance
/// </summary>
/// <returns></returns>
public delegate MqttFactory MqttFactoryProvider();