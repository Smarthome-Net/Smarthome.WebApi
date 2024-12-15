using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.Services;

namespace SmartHome.MongoService.Extension;

public static class SettingServiceExtension
{
    public static Task<long> UpdateCommonSetting(this ISettingService settingService, CommonSetting commonSetting) 
    {
        var updateDefinition = Builders<CommonSetting>.Update
            .Set(f => f.Description, commonSetting.Description)
            .Set(f => f.Title, commonSetting.Title)
            .Set(f => f.PageLength, commonSetting.PageLength)
            .Set(f => f.ColorScheme, commonSetting.ColorScheme);
        return settingService.UpdateSetting(commonSetting, updateDefinition);
    }
}
