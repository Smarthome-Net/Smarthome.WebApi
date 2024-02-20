using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureStatisticService
{
    Chart<NamedSeries> GetStatistic(StatisticRequest request);
}
