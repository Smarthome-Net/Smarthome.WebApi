using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.DTO.Requests;
using SmartHome.Common.Models.DTO.Responses;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemperatureChartController : ControllerBase
{
    private readonly ILogger<TemperatureChartController> logger;
    private readonly ITemperatureReaderService temperatureService;
    public TemperatureChartController(ILogger<TemperatureChartController> logger, 
        ITemperatureReaderService temperatureService)
    {
        this.logger = logger;
        this.temperatureService = temperatureService;
    }
    
    [Produces("application/json")]
    [HttpPost]
    public ActionResult<TemperatureResponse> GetTemperature(TemperatureRequest temperatureRequest) 
    {
        TemperatureResponse temperatureResponse = new()
        {
            Scope = temperatureRequest.Scope,
            Temperatures = temperatureService.GetTemperature(temperatureRequest),
            PageSetting = temperatureRequest.PageSetting
        };
        return Ok(temperatureResponse);
    }

}
