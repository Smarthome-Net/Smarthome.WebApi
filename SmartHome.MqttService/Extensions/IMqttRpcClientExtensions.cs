using MQTTnet.Extensions.Rpc;
using MQTTnet.Protocol;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartHome.MqttService.Extensions;

internal static class IMqttRpcClientExtensions
{
    public static async Task<T?> ExecuteAsync<T>(this IMqttRpcClient mqttRpcClient, TimeSpan timeout, string methodName, T? payload, MqttQualityOfServiceLevel qos) 
    {
        var data = Array.Empty<byte>();
        if(payload is not null)
        {
            data = JsonSerializer.SerializeToUtf8Bytes(payload);
        }
        
        var rawResponse = await mqttRpcClient.ExecuteAsync(timeout, methodName, data, qos);
        var response = Encoding.UTF8.GetString(rawResponse);

        return response is null 
            ? throw new NullReferenceException("Device doesn't respond with any data") 
            : JsonSerializer.Deserialize<T>(response);
    }
}
