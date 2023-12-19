using System.Collections.Generic;
using SmartHome.Common.Models.DTO.Charts;

namespace SmartHome.Common.Models.DTO;

public class FilterableChart<TSeries>
{
    public FilterableChart(string scopeFilter, IEnumerable<Chart<TSeries>> chart)
    {
        ScopeFilter = scopeFilter;
        Chart = chart;
    }

    public string ScopeFilter { get; }
    public IEnumerable<Chart<TSeries>> Chart { get; }
}
