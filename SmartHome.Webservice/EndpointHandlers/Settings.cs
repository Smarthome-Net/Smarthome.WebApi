using System.Collections.Generic;
using SmartHome.Common.Interfaces;
using System.Threading.Tasks;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.MongoService.Extension;

namespace SmartHome.Webservice.EndpointHandlers;

public static class Settings
{
    public static async Task<IEnumerable<SettingDto>> GetAllSetting(ISettingService settingService)
    {
        var settings = await settingService.GetAllSetting();
        return settings.ToDto();
    }
    
    public static async Task<CommonSettingDto> GetCommonSetting(ISettingService settingService)
    {
        var setting = await settingService.GetSetting<CommonSetting>();
        return setting.ToDto();
    }
    
    public static async Task<long> UpdateCommonSetting(CommonSettingDto commonSetting, ISettingService settingService)
    {
        var setting = commonSetting.ToDb();
        if (setting == null)
        {
            return -1;
        }
        
        return await settingService.UpdateCommonSetting(setting);
    }
}
