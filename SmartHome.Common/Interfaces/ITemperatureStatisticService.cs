using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureStatisticService
{
    Chart<string, float> GetStatistic(StatisticRequest request);
}
