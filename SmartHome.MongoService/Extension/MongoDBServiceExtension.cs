using Microsoft.Extensions.DependencyInjection;
using System;
using SmartHome.MongoService.DbContext;
using SmartHome.Common.Interfaces;
using SmartHome.MongoService.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

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
            var option = provider.GetRequiredService<IOptions<MongoDbOptions>>();
            var logger = provider.GetRequiredService<ILogger<MongoDBContext>>();
            var setting = option.Value.DbConnectionSetting;
            var mongoClient = new MongoClient(setting.GetMongoConnectionString());
            return new MongoDBContext(mongoClient, setting.Database, logger);
        });

        services.AddTransient<ITemperatureWriterService, TemperatureWriterService>();
        services.AddTransient<ITemperatureReaderService, TemperatureReaderService>();
        services.AddTransient<IDeviceService, DeviceService>();
        services.AddTransient<ISettingService, SettingService>();

        return services;
    }
}
