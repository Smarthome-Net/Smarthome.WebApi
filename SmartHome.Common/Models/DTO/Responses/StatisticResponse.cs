using SmartHome.Common.Models.DTO.Charts;

namespace SmartHome.Common.Models.DTO.Responses;

public class StatisticResponse
{
    public Scope Scope { get; set; }

    public Chart<NamedSeries> Statistic { get; set; }
}
