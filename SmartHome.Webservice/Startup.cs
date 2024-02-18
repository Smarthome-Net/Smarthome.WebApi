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
using SmartHome.Webservice.Helper;
using SmartHome.MongoService.Settings;

namespace SmartHome.Webservice;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var mqttSetting = Configuration.GetSection("MqttSetting").Get<MqttSetting>();
        var config = Configuration.GetSection("DbConnectionSetting").Get<DbConnectionSetting>();


        services.AddSingleton<ITemperatureHubQueue, TemperatureHubQueue>();
        
        services.AddMongoDbService(o =>
        {
            o.DbConnectionSetting = config;
        });

        services.AddMqttClientHostedService(o =>
        {
            o.MqttSetting = mqttSetting;
        });

        services.AddControllers()
            .AddJsonOptions(options => 
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        services.AddSignalR();


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

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<TemperatureChartHub>("/hub/temperature");
        });

    }
}
