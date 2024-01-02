using Microsoft.Extensions.Hosting;
using MQTTnet.Extensions.Rpc;
using SmartHome.Common.Models.Db;
using System;

namespace SmartHome.MqttService.Services;

public interface IMqttClientService : IDisposable, IHostedService
{
    IObservable<Temperature> Temperature { get; }

    IMqttRpcClient CreateMqttRpcClient();
}
