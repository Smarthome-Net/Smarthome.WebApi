using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Extensions;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Dto.Requests;
using SmartHome.Common.Models.Dto.Responses;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticController : ControllerBase
{
    private readonly ITemperatureReaderService statisticService;
    private readonly ILogger<StatisticController> logger;

    public StatisticController(ILogger<StatisticController> logger, ITemperatureReaderService statisticService) 
    {
        this.logger = logger;
        this.statisticService = statisticService;
    }

    [HttpPost]
    public ActionResult<StatisticResponse> GetStatistic(StatisticRequest request)
    {
        var temperatures = statisticService.GetTemperature(request.Scope);
        var response = new StatisticResponse
        {
            Scope = request.Scope,
            Statistic = temperatures.ToStatisticChart(request.Scope)
        };
        return Ok(response);
    }
}
