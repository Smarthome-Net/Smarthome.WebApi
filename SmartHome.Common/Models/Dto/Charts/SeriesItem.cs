namespace SmartHome.Common.Models.Dto.Charts;

/// <summary>
/// Generic series item for the chart series collection
/// </summary>
/// <typeparam name="TName"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class SeriesItem<TName, TValue> : SeriesItem
{
    public required TName Name { get; set; }
    public required TValue Value { get; set; }
}


/// <summary>
/// Non-generic base class for the generic variant
/// </summary>
public class SeriesItem
{
    /// <summary>
    /// Create a new series item for a chart series collection
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <typeparam name="TName"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public static SeriesItem<TName, TValue> Create<TName, TValue>(TName name, TValue value)
    {
        return new SeriesItem<TName, TValue>
        {
            Name = name,
            Value = value
        };
    }
}
