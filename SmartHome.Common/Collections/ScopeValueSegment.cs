using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SmartHome.Common.Models;

namespace SmartHome.Common.Collections;

/// <summary>
/// This is only some syntactic sugar, to improve the handling for value of the <see cref="Scope"/>
/// </summary>
public class ScopeValueSegment : IEnumerable<Segment>
{
    public const char SegmentSeperator = '/';
    
    private readonly List<Segment> _scopeSegements = new();

    public Segment this[int index]
    {
        get { return _scopeSegements[index]; }
        set { _scopeSegements[index] = value; }
    }

    /// <summary>
    /// Adds a new raw segment
    /// </summary>
    /// <param name="value"></param>
    public void AddSegment(string value) 
    {
        _scopeSegements.Add(new Segment(value));
    }

    /// <summary>
    /// Adds a new segment
    /// </summary>
    /// <param name="segment"></param>
    public void AddSegment(Segment segment) 
    {
        _scopeSegements.Add(segment);
    }

    /// <summary>
    /// Merge the segments into string, with the seperator
    /// </summary>
    /// <returns></returns>
    public string MergeSegments() 
    {
        return string.Join(SegmentSeperator, _scopeSegements.Select(seg => seg.Value));
    }

    /// <summary>
    /// Forward property to the count value of the inner collection
    /// </summary>
    public int Count => _scopeSegements.Count;

    /// <summary>
    /// Forward property to the capacity value of the inner collection
    /// </summary>
    public int Capacity => _scopeSegements.Capacity;

    public IEnumerator<Segment> GetEnumerator()
    {
        return _scopeSegements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _scopeSegements.GetEnumerator();
    }
}
