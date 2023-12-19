using SmartHome.Common.Models.DTO;
using SmartHome.Common.Models.DTO.Charts;
using SmartHome.Common.Models.DTO.Requests;
using System.Collections.Generic;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureStatisticService
{
    IEnumerable<Chart<NamedSeries>> GetStatistic(StatisticRequest request);
}
