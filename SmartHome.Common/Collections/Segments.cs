using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Collections;

/// <summary>
/// This is only some syntactic sugar, to improve the handling for value of the <see cref="Scope"/> and the mqtt topic
/// </summary>
public class Segments : IEnumerable<Segment>
{
    private const char SegmentSeparator = '/';
    
    private readonly List<Segment> _segments = [];

    public Segment this[int index]
    {
        get => _segments[index]; 
        set => _segments[index] = value;
    }

    /// <summary>
    /// Create a new instance from the string value, containing all segments from the string
    /// </summary>
    /// <param name="value">A value with one or more forward slashes, that can be split</param>
    /// <returns></returns>
    public static Segments FromString(string value) 
    {
        var segments = value
            .Split(SegmentSeparator)
            .Select(s => new Segment(s));
        return [..segments];
    }

    /// <summary>
    /// Adds a new segment
    /// </summary>
    /// <param name="segment"></param>
    private void Add(Segment segment) 
    {
        _segments.Add(segment);
    }

    /// <summary>
    /// Merge the segments into a raw string
    /// </summary>
    /// <returns></returns>
    public string MergeSegments() 
    {
        return string.Join(SegmentSeparator, _segments.Select(seg => seg.Value));
    }

    /// <summary>
    /// Remove segments from the collection
    /// </summary>
    /// <param name="value">The value, can also contain the segment separator to remove multiple segments</param>
    public void RemoveSegments(string value)
    {
        var segmentsToRemove = FromString(value);

        foreach (var segment in segmentsToRemove)
        {
            _segments.Remove(segment);
        }
    }

    /// <summary>
    /// Forward property to the count value of the inner collection
    /// </summary>
    public int Count => _segments.Count;

    /// <summary>
    /// Forward property to the capacity value of the inner collection
    /// </summary>
    public int Capacity => _segments.Capacity;

    public IEnumerator<Segment> GetEnumerator()
    {
        return _segments.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _segments.GetEnumerator();
    }
}
