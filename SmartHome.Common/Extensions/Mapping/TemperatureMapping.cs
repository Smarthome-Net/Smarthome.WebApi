using MongoDB.Bson;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Extensions.Mapping;

public static class TemperatureMapping
{
    public static TemperatureDto ToDto(this Temperature temperature)
    {
        return new TemperatureDto
        {
            Id = temperature.Id.ToString(),
            RecordDateTime = temperature.RecordDateTime,
            Value = temperature.Value,
        };
    }

    public static Temperature? ToDb(this TemperatureDto temperature)
    {
        if (!ObjectId.TryParse(temperature.Id, out var id))
        {
            return null;
        }

        return new Temperature
        {
            Id = id,
            RecordDateTime = temperature.RecordDateTime,
            Value = temperature.Value,
        };
    }
}
