using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Common.Models.DTO;

namespace SmartHome.Webservice.Hubs.Interfaces;

public interface ITemperatureChartHub
{
    Task UpdateTemperatuure(IEnumerable<Chart<TimeSeries>> chartData);

    Task SendMessage(string message);
}
