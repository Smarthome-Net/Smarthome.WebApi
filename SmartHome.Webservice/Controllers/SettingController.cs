using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Common.Interfaces;
using System.Threading.Tasks;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.MongoService.Extension;

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
    public async Task<IEnumerable<SettingDto>> GetAllSetting()
    {
        var settings = await _settingService.GetAllSetting();
        return settings.ToDto();
    }
    
    [HttpGet("CommonSetting")]
    public async Task<CommonSettingDto> GetCommonSetting()
    {
        var setting = await _settingService.GetSetting<CommonSetting>();
        return setting.ToDto();
    }
    
    [HttpPost("CommonSetting")]
    public async Task<long> UpdateCommonSetting(CommonSettingDto commonSetting)
    {
        var setting = commonSetting.ToDb();
        if (setting == null)
        {
            return -1;
        }
        
        return await _settingService.UpdateCommonSetting(setting);
    }
}
