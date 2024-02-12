using MQTTnet.Extensions.Rpc;
using SmartHome.MqttService.Settings;
using System;

namespace SmartHome.MqttService;

internal class SmarthomeRpcTopicGenerationStrategy : IMqttRpcClientTopicGenerationStrategy
{
    private readonly MqttSetting _mqttSetting;

    public SmarthomeRpcTopicGenerationStrategy(MqttSetting mqttSetting)
    {
        _mqttSetting = mqttSetting;
    }
    public MqttRpcTopicPair CreateRpcTopics(TopicGenerationContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (context.MethodName.Contains('+') || context.MethodName.Contains('#'))
        {
            throw new ArgumentException("The method name cannot contain + or #.");
        }

        var baseTopic = _mqttSetting?.TopicSetting?.SubscriptionTopic!.TrimEnd('#').TrimEnd('/');

        var requestTopic = $"{baseTopic}.RPC/{context.MethodName}";
        var responseTopic = requestTopic + "/response";

        return new MqttRpcTopicPair
        {
            RequestTopic = requestTopic,
            ResponseTopic = responseTopic,
        };
    }
}
