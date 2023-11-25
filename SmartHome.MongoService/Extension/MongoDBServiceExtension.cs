using Microsoft.Extensions.DependencyInjection;
using System;
using SmartHome.MongoService.Provider;
using SmartHome.Common.Interfaces;
using SmartHome.MongoService.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;

namespace SmartHome.MongoService.Extension;

public static class MongoDBServiceExtension
{
    public static IServiceCollection AddMongoDbService(this IServiceCollection services, Action<MongoDbOptions> options) 
    {
        services
            .AddOptions<MongoDbOptions>()
            .Configure(options);

        services.AddSingleton(provider =>
        {
            var settings = provider.GetService<IOptions<MongoDbOptions>>();
            return new MongoDBConnectionProvider(settings.Value.DbConnectionSetting);
        });

        services.AddTransient<ITemperatureWriterService, TemperatureWriterService>();
        services.AddTransient<ITemperatureReaderService, TemperatureReaderService>();
        services.AddTransient<IDeviceService, DeviceService>();

        return services;
    }
}
