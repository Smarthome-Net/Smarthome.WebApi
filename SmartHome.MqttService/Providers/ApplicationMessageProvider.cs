using Microsoft.Extensions.DependencyInjection;
using SmartHome.Common.Interfaces;
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
        return _provider.GetService<IApplicationMessageProcessor<TMessage>>();
    }
}
