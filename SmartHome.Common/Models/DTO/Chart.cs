using System.Collections.Generic;

namespace SmartHome.Common.Models.DTO;

public class Chart
{
    public string Name { get; set; }
    public IEnumerable<TimeSeries> Series { get; set; }
}
