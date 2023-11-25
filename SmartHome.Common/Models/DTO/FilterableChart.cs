using System.Collections.Generic;

namespace SmartHome.Common.Models.DTO;

public class FilterableChart
{
    public FilterableChart(string scopeFilter, IEnumerable<Chart> chart)
    {
        ScopeFilter = scopeFilter;
        Chart = chart;
    }

    public string ScopeFilter { get; }
    public IEnumerable<Chart> Chart { get; }
}
