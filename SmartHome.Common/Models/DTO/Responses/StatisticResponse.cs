using SmartHome.Common.Models.Dto.Charts;

namespace SmartHome.Common.Models.Dto.Responses;

public class StatisticResponse
{
    public Scope Scope { get; set; }

    public Chart<NamedSeries> Statistic { get; set; }
}
