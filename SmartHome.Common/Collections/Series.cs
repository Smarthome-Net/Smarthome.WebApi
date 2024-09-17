using System.Collections;
using System.Collections.Generic;
using SmartHome.Common.Models.Dto.Charts;

namespace SmartHome.Common.Collections;

public class Series<TName, TValue> : IEnumerable<SeriesItem<TName, TValue>>
{
    private readonly List<SeriesItem<TName, TValue>> _seriesItem = [];

    public SeriesItem<TName, TValue> this[int index]
    {
        get { return _seriesItem[index]; }
        set { _seriesItem[index] = value; }
    }

    public static Series<TName, TValue> From(IEnumerable<SeriesItem<TName, TValue>> items) 
    {
        return new Series<TName, TValue>(items);
    }
    
    public Series(){}

    private Series(IEnumerable<SeriesItem<TName, TValue>> items) 
    {
        _seriesItem.AddRange(items);
    }

    public void Add(SeriesItem<TName, TValue> item) 
    {
        _seriesItem.Add(item);
    }

    public void AddRange(IEnumerable<SeriesItem<TName, TValue>> items) 
    {
        _seriesItem.AddRange(items);
    }

    public void Remove(SeriesItem<TName, TValue> item) 
    {
        _seriesItem.Remove(item);
    }

    public void RemoveRange(int index, int count) 
    {
        _seriesItem.RemoveRange(index, count);
    }

    /// <summary>
    /// Forward property to the count value of the inner collection
    /// </summary>
    public int Count => _seriesItem.Count;

    /// <summary>
    /// Forward property to the capacity value of the inner collection
    /// </summary>
    public int Capacity => _seriesItem.Capacity;


    public IEnumerator<SeriesItem<TName, TValue>> GetEnumerator()
    {
        return _seriesItem.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _seriesItem.GetEnumerator();
    }
}
