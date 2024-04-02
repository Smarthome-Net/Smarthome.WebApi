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
        ArgumentNullException.ThrowIfNull(context);

        if (context.MethodName.Contains('+') || context.MethodName.Contains('#'))
        {
            throw new ArgumentException("The method name cannot contain + or #.");
        }

        var requestTopic = $"{_mqttSetting?.TopicSetting?.SubscriptionRpcTopic}/{context.MethodName}";
        var responseTopic = requestTopic + "/response";

        return new MqttRpcTopicPair
        {
            RequestTopic = requestTopic,
            ResponseTopic = responseTopic,
        };
    }
}
