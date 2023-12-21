using SmartHome.Common.Models.DTO.Charts;
using System.Collections.Generic;

namespace SmartHome.Common.Models.DTO.Responses;

public class StatisticResponse
{
    public Scope Scope { get; set; }

    public IEnumerable<Chart<NamedSeries>> Statistics { get; set; }
}
