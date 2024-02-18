using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHome.MqttService.Providers;
using SmartHome.MqttService.Services;
using SmartHome.MqttService.Settings;
using System;
using MQTTnet.Client;
using Microsoft.Extensions.Options;

namespace SmartHome.MqttService.Extensions;

public static class MqttClientServiceExtension
{
    public static IServiceCollection AddMqttClientHostedService(this IServiceCollection services, Action<MqttOptions> configuration)
    {
        services.AddOptions<MqttOptions>()
            .Configure(configuration);
        services.AddMqttClientServiceWithConfig((optionsBuilder, serviceProvider) =>
        {
            var mqttOptions = serviceProvider.GetRequiredService<IOptions<MqttOptions>>();
            var settings = mqttOptions.Value.MqttSetting!;
            optionsBuilder
                .WithCredentials(settings?.ClientSetting?.UserName, settings?.ClientSetting?.Password)
                .WithClientId(settings?.ClientSetting?.Id)
                .WithTcpServer(settings?.BrokerSetting?.Host, settings?.BrokerSetting?.Port);
        });
        return services;
    }

    private static IServiceCollection AddMqttClientServiceWithConfig(this IServiceCollection services, Action<MqttClientOptionsBuilder, ServiceProvider> optionsBuilder)
    {
        services.AddApplicationMessageProcessors();
        services.AddTransient(serviceProvider =>
        {
            var optionBuilder = new MqttClientOptionsBuilder();
            optionsBuilder(optionBuilder, services.BuildServiceProvider());
            return optionBuilder.Build();
        });

        services.AddSingleton<IMqttClientService, MqttClientService>();
        services.AddSingleton<IHostedService>(serviceProvider =>
        {
            return serviceProvider.GetRequiredService<IMqttClientService>();
        });

        services.AddSingleton(serviceProvider =>
        {
            var mqttClientService = serviceProvider.GetRequiredService<IMqttClientService>();
            var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
            return mqttClientServiceProvider;
        });

        services.AddTransient<IDeviceManager, DeviceManager>();
        return services;
    }
}
