using Microsoft.AspNetCore.Mvc;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using System.Threading.Tasks;

namespace SmartHome.Webservice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SettingController : ControllerBase
{
    private readonly ISettingService _settingService;
    
    public SettingController(ISettingService settingService)
    {
        _settingService = settingService;
    }


    [HttpGet]
    public async Task<Setting> GetSetting() 
    {
        return await _settingService.GetSetting<PageSetting>("66f4ee814b3a5a0e5074b591");
    }
}
