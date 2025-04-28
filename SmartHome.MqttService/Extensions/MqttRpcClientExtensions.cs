using MQTTnet.Extensions.Rpc;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHome.MqttService.Extensions;

internal static class MqttRpcClientExtensions
{
    private static JsonSerializerOptions SerializerOptions => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    
    /// <summary>
    /// Execute the action with the given data and returns the response
    /// </summary>
    /// <param name="mqttRpcClient"></param>
    /// <param name="timeout"></param>
    /// <param name="methodName"></param>
    /// <param name="payload"></param>
    /// <param name="qos"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T?> ExecuteAsync<T>(this IMqttRpcClient mqttRpcClient, TimeSpan timeout, string methodName, T payload, MqttQualityOfServiceLevel qos)
    {
        var data = JsonSerializer.SerializeToUtf8Bytes(payload);
        return await mqttRpcClient.ExecuteAsync<T>(timeout, methodName, qos, data);
    }

    /// <summary>
    /// Execute the action without any data and returns the response
    /// </summary>
    /// <param name="mqttRpcClient"></param>
    /// <param name="timeout"></param>
    /// <param name="methodName"></param>
    /// <param name="qos"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T?> ExecuteAsync<T>(this IMqttRpcClient mqttRpcClient, TimeSpan timeout, string methodName, MqttQualityOfServiceLevel qos) 
    {
        byte[] data = []; //use an empty list      
        return await mqttRpcClient.ExecuteAsync<T>(timeout, methodName, qos, data);
    }

    private static async Task<T?> ExecuteAsync<T>(this IMqttRpcClient mqttRpcClient, TimeSpan timeout, string methodName, MqttQualityOfServiceLevel qos, byte[] data)
    {
        var rawResponse = await mqttRpcClient.ExecuteAsync(timeout, methodName, data, qos);
        var response = Encoding.UTF8.GetString(rawResponse);
        
        return response is null
            ? throw new NullReferenceException("Device doesn't respond with any data")
            : JsonSerializer.Deserialize<T>(response, SerializerOptions);
    }
}
