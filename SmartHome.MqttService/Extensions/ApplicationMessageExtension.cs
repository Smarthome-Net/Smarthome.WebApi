using Microsoft.Extensions.DependencyInjection;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;

namespace SmartHome.MqttService.Extensions;

public static class ApplicationMessageExtension
{
    public static IServiceCollection AddApplicationMessageProcessors(this IServiceCollection services) 
    {
        _ = services.AddTransient<IApplicationMessageProcessor<Temperature>, TemperatureMessageProcessor>();
        return services;
    }
}
