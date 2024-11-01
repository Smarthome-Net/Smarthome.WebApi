using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Common.Interfaces;
using System.Threading.Tasks;
using MongoDB.Driver;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

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
        return settings.ToDto().ToList();
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
        var dbModel = commonSetting.ToDb();
        if (dbModel == null)
        {
            return -1;
        }
        
        var updateDefinition = Builders<CommonSetting>.Update
            .Set(f => f.Description, commonSetting.Description)
            .Set(f => f.Title, commonSetting.Title)
            .Set(f => f.ColorScheme, commonSetting.ColorScheme);
        return await _settingService.UpdateSetting(dbModel, updateDefinition);
    }
}
