using System.Threading;
using System.Threading.Tasks;
using MQTTnet;

namespace SmartHome.MqttService.MqttActions;

/// <summary>
/// Defines an action to process an application message
/// </summary>
public interface IMqttAction
{
    /// <summary>
    /// Process the action
    /// </summary>
    /// <param name="applicationMessage"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task ProcessAction(MqttApplicationMessage applicationMessage, CancellationToken token = default);
    
    /// <summary>
    /// The sensor type of the action, <see cref="SensorTypes"/>
    /// </summary>
    string SensorType { get; }
}
