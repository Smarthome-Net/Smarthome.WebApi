using SmartHome.MqttService.Services;

namespace SmartHome.MqttService.Providers;

/// <summary>
/// Factory delegate get an <see cref="IMqttClientService"/>
/// </summary>
/// <returns></returns>
public delegate IMqttClientService MqttClientServiceProvider();

