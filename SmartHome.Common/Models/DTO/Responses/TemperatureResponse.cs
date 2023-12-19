using System.Collections.Generic;
using SmartHome.Common.Models.DTO.Charts;

namespace SmartHome.Common.Models.DTO.Responses;

public class TemperatureResponse
{
    public Scope Scope { get; set; }
    public IEnumerable<Chart<TimeSeries>> Temperatures { get; set; }
    public PageSetting PageSetting { get; set; }
}
