using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Extensions;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.Common.Models.Dto.Responses;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureChartController : ControllerBase
{
    private readonly ILogger<TemperatureChartController> _logger;
    private readonly ITemperatureReaderService _temperatureService;
    public TemperatureChartController(ILogger<TemperatureChartController> logger, 
        ITemperatureReaderService temperatureService)
    {
        this._logger = logger;
        this._temperatureService = temperatureService;
    }
    
    [Produces("application/json")]
    [HttpPost]
    public ActionResult<TemperatureResponse> GetTemperature(TemperatureRequest temperatureRequest) 
    {
        var keySelector = temperatureRequest.Scope?.ToTemperatureKeySelector();
        var temperature = _temperatureService.GetTemperature(temperatureRequest.Scope);

        TemperatureResponse temperatureResponse = new()
        {
            Scope = temperatureRequest.Scope,
            Temperatures = temperature
                .ToTimeSeriesChart(keySelector!)
                .ApplyPaging(temperatureRequest.PageSetting!),
            PageSetting = temperatureRequest.PageSetting
        };
        return Ok(temperatureResponse);
    }

}
