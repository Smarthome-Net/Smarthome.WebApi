using SmartHome.Common.Models.Dto.Charts;

namespace SmartHome.Common.Helpers;

/// <summary>
/// Creation Helper for the series
/// </summary>
public static class SeriesHelper 
{
    /// <summary>
    /// Just create a new series item with the given parameter
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
            Value = value,
        };
    }
}