using Microsoft.Extensions.DependencyInjection;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.Providers;

namespace SmartHome.MqttService.Extensions;

public static class ApplicationMessageExtension
{
    public static IServiceCollection AddApplicationMessageProcessors(this IServiceCollection services) 
    {
        _ = services.AddTransient<IApplicationMessageProcessor<Temperature>, TemperatureMessageProcessor>();

        _ = services.AddTransient<IApplicationMessageProvider, ApplicationMessageProvider>();
        return services;
    }
}
