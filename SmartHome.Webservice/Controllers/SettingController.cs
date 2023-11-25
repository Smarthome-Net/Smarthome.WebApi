using Microsoft.AspNetCore.Mvc;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingController : ControllerBase
{
    public SettingController()
    {

    }


    [HttpGet]
    public IActionResult Index()
    {
        return Ok();
    }
}
