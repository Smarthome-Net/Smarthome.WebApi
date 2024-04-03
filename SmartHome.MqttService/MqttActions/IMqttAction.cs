using MQTTnet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.MqttService.MqttActions;

public interface IMqttAction : IDisposable
{
    Task ExecuteAction(MqttApplicationMessage message, CancellationToken token = default);
    
    string GetActionSubTopic(string fullTopic, string baseTopic);
}
