using Microsoft.Extensions.Hosting;
using MQTTnet.Extensions.Rpc;
using System;

namespace SmartHome.MqttService.Services;

public interface IMqttClientService : IDisposable, IHostedService
{
    IMqttRpcClient CreateMqttRpcClient();
}
