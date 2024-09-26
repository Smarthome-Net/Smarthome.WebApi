using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Extensions.Mapping;

public static class PageSettingMapping
{
    public static PageSettingDto ToDto(this PageSetting setting)
    {
        var pageSetting = setting.MapBaseDto<PageSetting, PageSettingDto>();
        pageSetting.Length = setting.Length;
        pageSetting.PageIndex = setting.PageIndex;
        pageSetting.PageSize = setting.PageSize;
        return pageSetting;
    }

    public static PageSetting? ToDb(this PageSettingDto setting)
    {
        var pageSetting = setting.MapBaseDb<PageSettingDto, PageSetting>();
        if(pageSetting == null)
        {
            return null;
        }

        pageSetting.Length = setting.Length;
        pageSetting.PageIndex = setting.PageIndex;
        pageSetting.PageSize = setting.PageSize;
        return pageSetting;
    }
}
