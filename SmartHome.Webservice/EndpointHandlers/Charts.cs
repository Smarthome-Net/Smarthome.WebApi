using Microsoft.Extensions.Logging;
using SmartHome.Common.Extensions;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.Common.Models.Dto.Responses;

namespace SmartHome.Webservice.EndpointHandlers;

public class Charts
{
    public static TemperatureResponse GetTemperature(TemperatureRequest temperatureRequest, ITemperatureService temperatureService)
    {
        var keySelector = temperatureRequest.Scope?.ToTemperatureKeySelector();
        var temperature = temperatureService.GetTemperature(temperatureRequest.Scope);

        TemperatureResponse temperatureResponse = new()
        {
            Scope = temperatureRequest.Scope,
            Temperatures = temperature
                .ToTimeSeriesChart(keySelector!)
                .ApplyPaging(temperatureRequest.Pagination!),
            Pagination = temperatureRequest.Pagination
        };
        return temperatureResponse;
    }

    public static StatisticResponse GetStatistic(StatisticRequest request, ILogger<Charts> logger, ITemperatureService statisticService)
    {
        logger.LogInformation("Get statistic for scope: {Value}", request.Scope!.Value);
        var temperatures = statisticService.GetTemperature(request.Scope);
        return new StatisticResponse
        {
            Scope = request.Scope,
            Statistic = temperatures.ToStatisticChart(request.Scope)
        };
    }
}
