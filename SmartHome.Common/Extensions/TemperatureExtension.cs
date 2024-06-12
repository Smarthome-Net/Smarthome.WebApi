using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Helpers;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Common.Extensions;

public static class TemperatureExtension
{
    public static IEnumerable<Series<DateTimeOffset, float>> ApplyPaging(this IEnumerable<Series<DateTimeOffset, float>> data, PageSetting setting)
    {
        var itemsToSkip = setting.PageIndex * setting.PageSize;
        var itemsToTake = setting.PageSize;
        setting.Length = data.Count();

        return data
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .OrderByDescending(item => item.Name);
    }

    public static IEnumerable<Chart<DateTimeOffset, float>> ApplyPaging(this IEnumerable<Chart<DateTimeOffset, float>> data, PageSetting setting)
    {
        foreach (var item in data)
        {
            item.Series = item.Series.ApplyPaging(setting);
            yield return item;
        }
    }

    public static IEnumerable<Series<DateTimeOffset, float>> ToTimeSeries(this IEnumerable<Temperature> data)
    {
        return data
            .GroupBy(item => item.RecordDateTime.Ticks / TimeSpan.FromSeconds(10).Ticks)
            .Select(groupedValues =>
            {
                var firstValue = groupedValues.FirstOrDefault();
                return SeriesHelper.Create(firstValue!.RecordDateTime, groupedValues.Average(item => item.Value));
            });
    }

    public static Chart<string, float> ToStatisticChart(this IEnumerable<Temperature> data, Scope scope) 
    {
        var max = data.Max(x => x.Value);
        var min = data.Min(x => x.Value);
        var avg = data.Average(x => x.Value);

        return new Chart<string, float>
        {
            Name = scope.Value!,
            Series =
            [
                SeriesHelper.Create("min", min),
                SeriesHelper.Create("average", avg),
                SeriesHelper.Create("max", max),
            ],
        };
    }

    public static IEnumerable<Chart<DateTimeOffset, float>> ToTimeSeriesChart(this IEnumerable<Temperature> data, Func<Temperature, string> keySelector)
    {
        return data
            .GroupBy(
                keySelector,
                (key, values) => new Chart<DateTimeOffset, float>()
                {
                    Name = key,
                    Series = values.ToTimeSeries()
                });
    }
}