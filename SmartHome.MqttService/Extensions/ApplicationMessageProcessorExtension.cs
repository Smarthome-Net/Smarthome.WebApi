using Microsoft.Extensions.DependencyInjection;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;

namespace SmartHome.MqttService.Extensions;

internal static class ApplicationMessageProcessorExtension
{
    public static IServiceCollection AddApplicationMessageProcessors(this IServiceCollection services) 
    {
        _ = services.AddTransient<IApplicationMessageProcessor<Temperature>, TemperatureMessageProcessor>();
        return services;
    }
}
