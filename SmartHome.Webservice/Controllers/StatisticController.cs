using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartHome.Common.Interfaces;

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

    [HttpPost("device/{name}/{isRoom}")]
    public async Task<IActionResult> CompareStatistic(string name, bool isRoom, List<string> compareList = null) 
    {
        var result = await statisticService.GetStatistic(name, isRoom, compareList);
        return Ok(result);
    }
}
