using SmartHome.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureStatisticService
{
    Task<TemperatureStatistic> GetStatistic(string name,  bool isRoom, List<string> compareList);
}
