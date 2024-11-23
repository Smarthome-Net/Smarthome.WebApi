namespace SmartHome.Common.Models.Dto.Charts;

/// <summary>
/// Generic series item for the chart series collection
/// </summary>
/// <typeparam name="TName"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class SeriesItem<TName, TValue>
{
    public required TName Name { get; set; }
    public required TValue Value { get; set; }
}
