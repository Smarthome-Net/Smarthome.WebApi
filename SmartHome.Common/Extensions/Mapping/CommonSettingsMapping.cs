using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Extensions.Mapping;

public static class CommonSettingsMapping
{
    public static CommonSettingDto? ToDto(this CommonSetting setting)
    {
        var commonSetting = setting.ToDto<CommonSetting, CommonSettingDto>();
        commonSetting.Title = setting.Title;
        commonSetting.Theme = setting.Theme;
        commonSetting.PageLength = setting.PageLength;
        return commonSetting;
    }

    public static CommonSetting? ToDb(this CommonSettingDto setting)
    {
        var commonSetting = setting.ToDb<CommonSettingDto, CommonSetting>();
        if(commonSetting == null)
        {
            return null;
        }

        commonSetting.Title = setting.Title;
        commonSetting.Theme = setting.Theme;
        commonSetting.PageLength = setting.PageLength;
        return commonSetting;
    }
}