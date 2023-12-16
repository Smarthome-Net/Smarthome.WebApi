using System.Collections.Generic;

namespace SmartHome.Common.Models.DTO;

public class TemperatureResponse 
{
    public ScopeType Scope { get; set; }
    public string ScopeValue { get; set; }
    public IEnumerable<Chart> Temperatures { get; set; }
    public PageSetting PageSetting { get; set; }
}
