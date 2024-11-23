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
    private readonly ITemperatureReaderService _statisticService;
    private readonly ILogger<StatisticController> _logger;

    public StatisticController(ILogger<StatisticController> logger, ITemperatureReaderService statisticService) 
    {
        _logger = logger;
        _statisticService = statisticService;
    }

    [HttpPost]
    public ActionResult<StatisticResponse> GetStatistic(StatisticRequest request)
    {
        var temperatures = _statisticService.GetTemperature(request.Scope);
        var response = new StatisticResponse
        {
            Scope = request.Scope,
            Statistic = temperatures.ToStatisticChart(request.Scope)
        };
        return Ok(response);
    }
}
