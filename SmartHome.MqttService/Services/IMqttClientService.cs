using Microsoft.Extensions.Hosting;
using SmartHome.Common.Models.Db;
using System;

namespace SmartHome.MqttService.Services;

public interface IMqttClientService : IDisposable, IHostedService
{
    IObservable<Temperature> Temperature { get; }
}
