using System.Collections;
using System.Collections.Generic;
using SmartHome.Common.Models.Dto.Charts;

namespace SmartHome.Common.Collections;

/// <summary>
/// Generic custom collection for a chart series
/// </summary>
/// <typeparam name="TName">The label name</typeparam>
/// <typeparam name="TValue">The chart value</typeparam>
public class Series<TName, TValue> : IEnumerable<SeriesItem<TName, TValue>>
{
    private readonly List<SeriesItem<TName, TValue>> _seriesItems = [];
    
    /// <summary>
    /// Returns a single item of the series
    /// </summary>
    /// <param name="index"></param>
    public SeriesItem<TName, TValue> this[int index]
    {
        get => _seriesItems[index]; 
        set => _seriesItems[index] = value;
    }
    
    /// <summary>
    /// Adda a new item into the series
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    public void Add(TName name, TValue value) 
    {
        Add(SeriesItem.Create(name, value));
    }
    
    /// <summary>
    /// Adds new items into the series
    /// </summary>
    /// <param name="item"></param>
    public void Add(SeriesItem<TName, TValue> item) 
    {
        _seriesItems.Add(item);
    }
    
    /// <summary>
    /// Forward property to the count value of the inner collection
    /// </summary>
    public int Count => _seriesItems.Count;

    /// <summary>
    /// Forward property to the capacity value of the inner collection
    /// </summary>
    public int Capacity => _seriesItems.Capacity;
    
    /// <summary>
    /// Returns a generic enumerator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<SeriesItem<TName, TValue>> GetEnumerator()
    {
        return _seriesItems.GetEnumerator();
    }
    
    /// <summary>
    /// Returns an enumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _seriesItems.GetEnumerator();
    }
}
