using SmartHome.Common.Models;
using SmartHome.MqttService.Providers;
using MQTTnet.Extensions.Rpc;
using System;
using SmartHome.MqttService.Extensions;
using System.Threading.Tasks;
using MQTTnet.Protocol;

namespace SmartHome.MqttService.Services;

public class DeviceManager : IDeviceManager
{
    private readonly IMqttRpcClient _mqttRpcClient;

    public DeviceManager(MqttClientServiceProvider mqttClientServiceProvider) 
    {
        var mqttClientService = mqttClientServiceProvider.MqttClientService;
        _mqttRpcClient = mqttClientService.CreateMqttRpcClient();
    }

    public async Task<DeviceConfiguration?> GetConfiguration(string deviceId)
    {
        var methodName = GetMethodName(deviceId, "config");
        return await _mqttRpcClient.ExecuteAsync<DeviceConfiguration>(TimeSpan.FromSeconds(10), methodName, null, MqttQualityOfServiceLevel.AtMostOnce);
    }

    public async Task<DeviceStatus?> GetStatus(string deviceId)
    {
        var methodName = GetMethodName(deviceId, "status");
        return await _mqttRpcClient.ExecuteAsync<DeviceStatus>(TimeSpan.FromSeconds(10), methodName, null, MqttQualityOfServiceLevel.AtMostOnce);
    }

    public async Task<DeviceConfiguration?> PopulateConfiguration(string deviceId, DeviceConfiguration deviceConfiguration)
    {
        var methodName = GetMethodName(deviceId, "config");
        return await _mqttRpcClient.ExecuteAsync(TimeSpan.FromSeconds(10), methodName, deviceConfiguration, MqttQualityOfServiceLevel.AtMostOnce);
    }

    private static string GetMethodName(string deviceId, string context) 
    {
        return $"{deviceId}/{context}";
    }
}
