using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Common.Extensions;

public static class TemperatureExtension
{
    public static IEnumerable<TimeSeries> ApplayPaging(this IEnumerable<TimeSeries> data, PageSetting setting) 
    {
        var itemsToSkip = setting.PageIndex * setting.PageSize;
        var itemsToTake = setting.PageSize;

        return data
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .OrderByDescending(item => item.Name);
    }

    public static IEnumerable<TimeSeries> ToTimeSeries(this IEnumerable<Temperature> data)
    {
        return data
            .GroupBy(item => item.RecordDateTime.Ticks / TimeSpan.FromSeconds(10).Ticks)
            .Select(groupedValues =>
            {
                var firstValue = groupedValues.FirstOrDefault();
                return new TimeSeries()
                {
                    Name = firstValue!.RecordDateTime,
                    Value = groupedValues.Average(item => item.Value)
                };
            });
    }

    public static IEnumerable<Chart<TimeSeries>> ToTimeSeriesChart(this IEnumerable<Temperature> data, Func<Temperature, string> keySelector, PageSetting setting)
    {
        return data
            .GroupBy(
            keySelector,
            (key, values) => new Chart<TimeSeries>()
            {
                Name = key,
                Series = values
                    .ToTimeSeries()
                    .ApplayPaging(setting)
            });
    }

    public static IEnumerable<Chart<TimeSeries>> ToTimeSeriesChart(this IEnumerable<Temperature> data, Func<Temperature, string> keySelector, Func<Temperature, bool> predicate)
    {
        return data
            .Where(predicate)
            .GroupBy(
                keySelector,
                (key, values) => new Chart<TimeSeries>()
                {
                    Name = key,
                    Series = values.ToTimeSeries()
                });
    }
}