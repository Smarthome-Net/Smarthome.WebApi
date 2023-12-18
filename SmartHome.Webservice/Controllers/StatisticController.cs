using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.DTO;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticController : ControllerBase
{
    private readonly ITemperatureStatisticService statisticService;
    private readonly ILogger<StatisticController> logger;

    public StatisticController(ILogger<StatisticController> logger, ITemperatureStatisticService statisticService) 
    {
        this.logger = logger;
        this.statisticService = statisticService;
    }

    [HttpPost]
    public IActionResult GetStatistic(Scope scope)
    {
        var result = statisticService.GetStatistic(scope);
        return Ok(result);
    }
}
