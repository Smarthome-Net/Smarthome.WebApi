using SmartHome.Common.Collections;

namespace SmartHome.Common.Extensions;

public static class StringExtensions
{
    public static ScopeValueSegment SplitIntoScope(this string topic)
    {
        var result = new ScopeValueSegment();
        foreach (var value in topic.Split(ScopeValueSegment.SegmentSeperator))
        {
            result.AddSegment(value);
        }
        return result;
    }
}
