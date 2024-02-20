using System.Collections.Generic;
using SmartHome.Common.Models.Dto.Charts;

namespace SmartHome.Common.Models.Dto.Responses;

public class TemperatureResponse
{
    public Scope Scope { get; set; }
    public IEnumerable<Chart<TimeSeries>> Temperatures { get; set; }
    public PageSetting PageSetting { get; set; }
}
