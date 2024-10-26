using Microsoft.Extensions.DependencyInjection;
using SmartHome.Common.Models.Dto;
using SmartHome.MqttService.ApplicationMessageProcessors;

namespace SmartHome.MqttService.Extensions;

internal static class ApplicationMessageProcessorExtension
{
    public static IServiceCollection AddApplicationMessageProcessors(this IServiceCollection services) 
    {
        _ = services.AddTransient<IApplicationMessageProcessor<TemperatureDto>, TemperatureMessageProcessor>();
        return services;
    }
}
