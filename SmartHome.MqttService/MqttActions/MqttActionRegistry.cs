using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartHome.Common.Collections;
using SmartHome.MqttService.Extensions;
using SmartHome.MqttService.Settings;
using System;

namespace SmartHome.MqttService.MqttActions;

public class MqttActionRegistry
{
    private readonly TopicSetting _topicSetting;
    private readonly IServiceProvider _provider;

    public MqttActionRegistry(IServiceProvider provider, IOptions<MqttOptions> options)
    {
        _topicSetting = options.Value.MqttSetting!.TopicSetting!;
        _provider = provider;
    }

    public IMqttAction? GetAction(string topic)
    {
        var segments = Segments.FromString(topic);
        segments.RemoveSegments(_topicSetting.SubscriptionTopic);
        return _provider.GetKeyedService<IMqttAction>(segments[0].Value);
    }
}
