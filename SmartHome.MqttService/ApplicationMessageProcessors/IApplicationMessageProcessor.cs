using MQTTnet;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.MqttService.ApplicationMessageProcessors;

/// <summary>
/// A Generic Application Message Processor for MQTT.
/// 
/// Can be implemented for different application messages 
/// to process the message into the given <typeparamref name="TMessage"/>
/// 
/// The implementation can also write the data into the database
/// </summary>
/// <typeparam name="TMessage">Model class</typeparam>
public interface IApplicationMessageProcessor<TMessage> : IDisposable where TMessage : class 
{
    /// <summary>
    /// The Subscription topic of the mqtt service
    /// </summary>
    /// <param name="topic"></param>
    string SubscriptionTopic { set; }

    /// <summary>
    /// Processes the incoming <see cref="MqttApplicationMessage"/> into the <typeparamref name="TMessage"/>
    /// </summary>
    /// <param name="applicationMessage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TMessage> ProcessMessage(MqttApplicationMessage applicationMessage, CancellationToken cancellationToken = default);
}
