using SmartHome.Common.Models.DTO;
using System.Collections.Generic;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureStatisticService
{
    IEnumerable<Chart<StatisticSeries>> GetStatistic(Scope scope);
}
