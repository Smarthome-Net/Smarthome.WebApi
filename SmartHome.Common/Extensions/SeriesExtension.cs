using System.Collections.Generic;
using SmartHome.Common.Collections;
using SmartHome.Common.Models.Dto.Charts;

namespace SmartHome.Common.Extensions;

public static class SeriesExtension
{
    public static Series<TName, TValue> ToSeries<TName, TValue>(this IEnumerable<SeriesItem<TName, TValue>> items) 
    {
        return Series<TName, TValue>.From(items);
    }
}
