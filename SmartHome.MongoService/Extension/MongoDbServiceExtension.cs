using Microsoft.Extensions.DependencyInjection;
using System;
using SmartHome.MongoService.DbContext;
using SmartHome.Common.Interfaces;
using SmartHome.MongoService.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;

namespace SmartHome.MongoService.Extension;

public static class MongoDbServiceExtension
{
    public static IServiceCollection AddMongoDbService(this IServiceCollection services, Action<MongoDbOptions> options) 
    {
        services
            .AddOptions<MongoDbOptions>()
            .Configure(options);
        services.AddTransient(provider =>
        {
            var option = provider.GetRequiredService<IOptions<MongoDbOptions>>();
            var logger = provider.GetRequiredService<ILogger<MongDBManagementContext>>();
            var setting = option.Value.DbConnectionSetting;
            var mongoClient = new MongoClient(setting.GetMongoConnectionString());
            return new MongDBManagementContext(mongoClient, setting.Database, logger);
        });

        services.AddTransient(provider =>
        {
            var managementContext = provider.GetRequiredService<MongDBManagementContext>();
            var logger = provider.GetRequiredService<ILogger<MongoDbContext>>();
            return new MongoDbContext(managementContext, logger);
        });

        services.AddTransient<ITemperatureService, TemperatureService>();
        services.AddTransient<IDeviceService, DeviceService>();
        services.AddTransient<ISettingService, SettingService>();

        return services;
    }
}
