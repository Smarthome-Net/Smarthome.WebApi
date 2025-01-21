using Microsoft.Extensions.DependencyInjection;
using SmartHome.MqttService.Providers;
using SmartHome.MqttService.Services;
using System;
using MQTTnet.Client;
using Microsoft.Extensions.Options;
using SmartHome.Common.Collections;
using SmartHome.MqttService.MqttActions;
using SmartHome.MqttService.Observables;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto;
using SmartHome.MqttService.ApplicationMessageProcessors;

namespace SmartHome.MqttService.Extensions;

public static class MqttClientServiceExtension
{
    public static void AddMqttClientHostedService(this IServiceCollection services, Action<MqttOptions> configuration)
    {
        services.AddOptions<MqttOptions>()
            .Configure(configuration);
        
        services.AddMqttActions();
        services.AddApplicationMessageProcessors();
        
        services.AddMqttClientServiceWithConfig((optionsBuilder, serviceProvider) =>
        {
            var mqttOptions = serviceProvider.GetRequiredService<IOptions<MqttOptions>>();
            var settings = mqttOptions.Value.MqttSetting;
            optionsBuilder
                .WithCredentials(settings.ClientSetting.UserName, settings.ClientSetting.Password)
                .WithClientId(settings.ClientSetting.Id)
                .WithTcpServer(settings.BrokerSetting.Host, settings.BrokerSetting.Port);
        });
    }

    private static void AddMqttClientServiceWithConfig(this IServiceCollection services,
        Action<MqttClientOptionsBuilder, ServiceProvider> optionsBuilder)
    {
        services.AddTransient(_ =>
        {
            var optionBuilder = new MqttClientOptionsBuilder();
            optionsBuilder(optionBuilder, services.BuildServiceProvider());
            return optionBuilder.Build();
        });

        services.AddSingleton<IMqttFactoryProvider, MqttFactoryProvider>();
        services.AddSingleton<IMqttClientService, MqttClientService>();
        services.AddHostedService(serviceProvider => serviceProvider.GetRequiredService<IMqttClientService>());

        services.AddSingleton(serviceProvider =>
        {
            var mqttClientService = serviceProvider.GetRequiredService<IMqttClientService>();
            var mqttClientServiceProvider = new MqttClientServiceProvider(mqttClientService);
            return mqttClientServiceProvider;
        });

        services.AddTransient<IDeviceManager, DeviceManager>();
    }

    private static void AddApplicationMessageProcessors(this IServiceCollection services) 
    {
        services.AddTransient<IApplicationMessageProcessor<TemperatureDto>, TemperatureMessageProcessor>();
    }

    private static void AddMqttActions(this IServiceCollection services)
    {
        services.AddSingleton<ITemperatureObservable, TemperatureObservable>();
        services.AddKeyedTransient<IMqttAction, TemperatureMqttAction>(SensorType.Temperature);

        services.AddTransient<MqttActionProvider>(sp =>
        {
            return MqttActionProvider;

            IMqttAction? MqttActionProvider(string topic)
            {
                var options = sp.GetRequiredService<IOptions<MqttOptions>>();
                var topicSetting = options.Value.MqttSetting.TopicSetting;
                var segments = Segments.FromString(topic);
                segments.RemoveSegments(topicSetting.SubscriptionTopic!);
                return sp.GetKeyedService<IMqttAction>(segments[0].Value);
            }
        });
    }
}
