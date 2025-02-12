using SmartHome.MqttService.MqttActions;

namespace SmartHome.MqttService.Providers;

/// <summary>
/// Factory delegate to resolve an <see cref="IMqttAction"/> from DI Container
/// </summary>
/// <param name="key"></param>
/// <returns></returns> <summary>
/// </summary>
public delegate IMqttAction? MqttActionProvider(string key);