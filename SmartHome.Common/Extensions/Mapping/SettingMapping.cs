using MongoDB.Bson;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Extensions.Mapping;

public static class SettingMapping
{
    public static SettingDto ToDto(this Setting setting)
    {
        return new SettingDto
        {
            Id = setting.Id.ToString(),
            Discription = setting.Discription,
        };
    }

    public static Setting? ToDb(this SettingDto setting)
    {
        if (!ObjectId.TryParse(setting.Id, out var id))
        {
            return null;
        }

        return new Setting
        {
            Id = id,
            Discription = setting.Discription,
        };
    }

    internal static TDto MapBaseDto<TDb, TDto>(this TDb setting)
        where TDb : Setting
        where TDto : SettingDto
    {
        return (TDto)setting.ToDto();
    }

    internal static TDb? MapBaseDb<TDto, TDb>(this TDto setting)
        where TDb : Setting
        where TDto : SettingDto
    {

        return (TDb?)setting.ToDb();
    }
}
