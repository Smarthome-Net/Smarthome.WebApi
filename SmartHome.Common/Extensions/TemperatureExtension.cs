using SmartHome.Common.Helpers;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Common.Extensions;

public static class TemperatureExtension
{
    private static IEnumerable<SeriesItem<DateTimeOffset, float>> ApplyPaging(this IEnumerable<SeriesItem<DateTimeOffset, float>> data, PageSettingDto setting)
    {
        var itemsToSkip = setting.PageIndex * setting.PageSize;
        var itemsToTake = setting.PageSize;
        var seriesItems = data.ToArray();
        setting.Length = seriesItems.Length;

        return seriesItems
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .OrderByDescending(item => item.Name);
    }

    public static IEnumerable<Chart<DateTimeOffset, float>> ApplyPaging(this IEnumerable<Chart<DateTimeOffset, float>> data, PageSettingDto setting)
    {
        foreach (var item in data)
        {
            item.Series = item.Series
                .ApplyPaging(setting)
                .ToSeries();
            yield return item;
        }
    }

    private static IEnumerable<SeriesItem<DateTimeOffset, float>> ToTimeSeries(this IEnumerable<TemperatureDto> data)
    {
        return data
            .GroupBy(item => item.RecordDateTime.Ticks / TimeSpan.FromSeconds(10).Ticks)
            .Select(groupedValues =>
            {
                var firstValue = groupedValues.FirstOrDefault();
                return SeriesHelper.Create(firstValue!.RecordDateTime, groupedValues.Average(item => item.Value));
            });
    }

    public static Chart<string, float> ToStatisticChart(this IEnumerable<TemperatureDto> data, Scope? scope) 
    {
        var temperatures = data.ToArray();
        var max = temperatures.Max(x => x.Value);
        var min = temperatures.Min(x => x.Value);
        var avg = temperatures.Average(x => x.Value);

        return new Chart<string, float>
        {
            Name = scope?.Value!,
            Series = {
                { "min", min },
                { "average", avg },
                { "max", max}
            }
        };
    }

    public static IEnumerable<Chart<DateTimeOffset, float>> ToTimeSeriesChart(this IEnumerable<TemperatureDto> data, Func<TemperatureDto, string> keySelector)
    {
        return data
            .GroupBy(
                keySelector,
                (key, values) => new Chart<DateTimeOffset, float>()
                {
                    Name = key,
                    Series = values
                        .ToTimeSeries()
                        .ToSeries()
                });
    }
}