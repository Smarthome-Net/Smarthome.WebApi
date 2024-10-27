using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Extensions.Mapping;

public static class SettingMapping
{
    internal static TDto ToDto<TDb, TDto>(this TDb setting)
        where TDb : Setting
        where TDto : SettingDto, new()
    {
        return new TDto
        {
            Id = setting.Id.ToString(),
            Description = setting.Description,
        };
    }
    
    public static IEnumerable<SettingDto> ToDto(this IEnumerable<Setting> settings)
    {
        return settings.Select(setting => setting.ToDto<Setting, SettingDto>());
    }

    internal static TDb? ToDb<TDto, TDb>(this TDto setting)
        where TDb : Setting, new()
        where TDto : SettingDto
    {
        if (!ObjectId.TryParse(setting.Id, out var id))
        {
            return null;
        }

        return new TDb
        {
            Id = id,
            Description = setting.Description,
        };
    }
}
