using MQTTnet;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.MqttService.MqttActions;

public interface IMqttAction
{
    Task ExecuteAction(MqttApplicationMessage message, string deviceContext, CancellationToken token = default);
    
    string GetSensorType();
}
