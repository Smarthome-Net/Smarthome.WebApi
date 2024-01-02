using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHome.MqttService.Providers;
using SmartHome.MqttService.Services;
using SmartHome.MqttService.Settings;
using System;
using MQTTnet.Client;

namespace SmartHome.MqttService.Extensions;

public static class MqttClientServiceExtension
{
    public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services, MqttSetting configuration)
    {
        services.AddTransient(options => configuration);
        services.AddMqttClientServiceWithConfig(optionsBuilder =>
        {
            optionsBuilder
                .WithCredentials(configuration.ClientSetting.UserName, configuration.ClientSetting.Password)
                .WithClientId(configuration.ClientSetting.Id)
                .WithTcpServer(configuration.BrokerSetting.Host, configuration.BrokerSetting.Port);
        });
        return services;
    }

    private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<MqttClientOptionsBuilder> optionsBuilder)
    {
        services.AddApplicationMessageProcessors();
        services.AddTransient(serviceProvider =>
        {
            var optionBuilder = new MqttClientOptionsBuilder();
            optionsBuilder(optionBuilder);
            return optionBuilder.Build();
        });

        services.AddSingleton<IMqttClientService, MqttClientService>();
        services.AddSingleton<IHostedService>(serviceProvider =>
        {
            return serviceProvider.GetService<IMqttClientService>();
        });

        services.AddSingleton(serviceProvider =>
        {
            var mqttClientService = serviceProvider.GetService<IMqttClientService>();
            var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
            return mqttClientServiceProvider;
        });

        services.AddTransient<IDeviceManager, DeviceManager>();
        return services;
    }
}
