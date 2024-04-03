using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Common.Models;

namespace SmartHome.Common.Collections;

/// <summary>
/// This is only some syntactic sugar, to improve the handling for value of the <see cref="Scope"/> and the mqtt topic
/// </summary>
public class Segments : IEnumerable<Segment>
{
    public const char SegmentSeperator = '/';
    
    private readonly List<Segment> _segements = [];

    public Segment this[int index]
    {
        get { return _segements[index]; }
        set { _segements[index] = value; }
    }

    /// <summary>
    /// Create a new instance from the string value, containing all segments from the string
    /// </summary>
    /// <param name="value">A value with one or more forward slashes, that can be splited</param>
    /// <returns></returns>
    public static Segments FromString(string value) 
    {
        var segments = new Segments();
        foreach (var values in value.Split(SegmentSeperator))
        {
            segments.AddSegment(values);
        }
        return segments;
    }

    /// <summary>
    /// Adds a new raw segment
    /// </summary>
    /// <param name="value"></param>
    public void AddSegment(string value) 
    {
        AddSegment(new Segment(value));
    }

    /// <summary>
    /// Adds a new segment
    /// </summary>
    /// <param name="segment"></param>
    public void AddSegment(Segment segment) 
    {
        _segements.Add(segment);
    }

    /// <summary>
    /// Merge the segments into a raw string
    /// </summary>
    /// <returns></returns>
    public string MergeSegments() 
    {
        return string.Join(SegmentSeperator, _segements.Select(seg => seg.Value));
    }

    /// <summary>
    /// Remove segments from the collection
    /// </summary>
    /// <param name="value">The value, can also contain the segment seperator to remove multiple segments</param>
    public void RemoveSegments(string value)
    {
        var segmentsToRemove = FromString(value);

        foreach (var segement in segmentsToRemove)
        {
            _segements.Remove(segement);
        }
    }

    /// <summary>
    /// Forward property to the count value of the inner collection
    /// </summary>
    public int Count => _segements.Count;

    /// <summary>
    /// Forward property to the capacity value of the inner collection
    /// </summary>
    public int Capacity => _segements.Capacity;

    public IEnumerator<Segment> GetEnumerator()
    {
        return _segements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _segements.GetEnumerator();
    }
}
