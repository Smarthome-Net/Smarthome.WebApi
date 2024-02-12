using Microsoft.Extensions.DependencyInjection;
using SmartHome.MqttService.ApplicationMessageProcessors;
using System;

namespace SmartHome.MqttService.Providers;

public interface IApplicationMessageProvider
{
    IApplicationMessageProcessor<TMessage> GetApplicationMessageProcessor<TMessage>() where TMessage : class;
}
public class ApplicationMessageProvider : IApplicationMessageProvider
{
    private readonly IServiceProvider _provider;

    public ApplicationMessageProvider(IServiceProvider provider)
    {
        _provider = provider;
    }
    public IApplicationMessageProcessor<TMessage> GetApplicationMessageProcessor<TMessage>() where TMessage : class
    {
        return _provider.GetRequiredService<IApplicationMessageProcessor<TMessage>>();
    }
}
