using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartHome.Common.Extensions;
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
        var scopes = topic.SplitIntoScope();
        scopes.RemoveSegments(_topicSetting.SubscriptionTopic);
        return _provider.GetKeyedService<IMqttAction>(scopes[0].Value);
    }
}
