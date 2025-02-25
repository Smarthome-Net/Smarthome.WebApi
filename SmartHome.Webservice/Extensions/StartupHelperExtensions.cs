using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartHome.Webservice.Hubs;
using Microsoft.OpenApi.Models;
using SmartHome.MqttService.Extensions;
using SmartHome.MqttService.Settings;
using SmartHome.MongoService.Extension;
using System.Text.Json.Serialization;
using SmartHome.MongoService.BsonCustomSerializers;
using SmartHome.Webservice.Helper;
using SmartHome.MongoService.Settings;
using Microsoft.AspNetCore.Http.Json;

namespace SmartHome.Webservice.Extensions;

public static class StartupHelperExtensions
{
    public static void AddSmartHomeServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITemperatureHubQueue, TemperatureHubQueue>();

        services.AddMongoDbService(o =>
        {
            var connectionSetting = configuration.GetSection("DbConnectionSetting").Get<DbConnectionSetting>();
            o.DbConnectionSetting = connectionSetting;
        }).ConfigureSerializer(s => { s.AddBsonSerializationProvider(new SmartHomeSerializerProvider()); });

        services.AddMqttClientHostedService(o =>
        {
            var mqttSetting = configuration.GetSection("MqttSetting").Get<MqttSetting>();
            o.MqttSetting = mqttSetting;
        });

        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });
        services.AddSignalR();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Smarthome Dashboard API", Version = "V1" });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }

    public static WebApplication ConfigureSmartHomeApp(this WebApplication app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors("CorsPolicy");
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DisplayRequestDuration();
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smarthome Dashboard API V1");
        });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.MapApiEndpoints();
        app.MapHub<TemperatureChartHub>("/hub/temperature");
        return app;
    }
}