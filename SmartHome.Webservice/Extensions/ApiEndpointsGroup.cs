using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using SmartHome.Webservice.EndpointHandlers;

namespace SmartHome.Webservice.Extensions;

public static class ApiEndpointsGroup
{
    public static WebApplication MapApiEndpoints(this WebApplication app)
    {
        var apiGroup = app.MapGroup("api");

        apiGroup.MapChartEndpoints();
        apiGroup.MapSettingEndpoints();
        apiGroup.MapDeviceEndpoints();

        return app;
    }

    private static void MapChartEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var chartGroup = endpoint.MapGroup("chart");

        chartGroup.MapPost("temperature", Charts.GetTemperature);
        chartGroup.MapPost("statistic", Charts.GetStatistic);
    }


    private static void MapDeviceEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var deviceGroup = endpoint.MapGroup("device");

        deviceGroup.MapGet("/", Devices.GetAllDevices);
        deviceGroup.MapGet("{room}", Devices.GetListOfDevices);
        deviceGroup.MapGet("{deviceId}/status", Devices.GetDeviceStatus);
        deviceGroup.MapGet("{deviceId}/config", Devices.GetDeviceConfig);
        deviceGroup.MapPost("{deviceId}/config", Devices.UpdateDeviceConfig);
    }

    private static void MapSettingEndpoints(this IEndpointRouteBuilder endpoint)
    {
        var settingGroup = endpoint.MapGroup("setting");

        settingGroup.MapGet("/", Settings.GetAllSetting);
        settingGroup.MapGet("commonsetting", Settings.GetCommonSetting);
        settingGroup.MapPost("commonsetting", Settings.UpdateCommonSetting);
    }
}
