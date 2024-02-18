using SmartHome.Common.Models.DTO.Charts;
using SmartHome.Common.Models.DTO.Requests;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureStatisticService
{
    Chart<NamedSeries> GetStatistic(StatisticRequest request);
}
