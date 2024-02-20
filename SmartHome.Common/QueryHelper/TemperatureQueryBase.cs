using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Common.QueryHelper;

public abstract class TemperatureQueryBase
{
    private static IEnumerable<TimeSeries> CreateTemperatureChartData(IEnumerable<Temperature> temperatureValues, PageSetting setting)
    {
        var groupedTemperatureValues = temperatureValues.GroupBy(item => item.RecordDateTime.Ticks / TimeSpan.FromSeconds(10).Ticks)
             .Select(groupedValues =>
             {
                 var firstValue = groupedValues.FirstOrDefault();
                 return new TimeSeries()
                 {
                     Name = firstValue.RecordDateTime,
                     Value = groupedValues.Average(item => item.Value)
                 };
             })
             .ToList();

        var itemsToSkip = setting.PageIndex * setting.PageSize;
        var itemsToTake = setting.PageSize;
        setting.Length = groupedTemperatureValues.Count;

        return groupedTemperatureValues
            .Skip(itemsToSkip)
            .Take(itemsToTake)
            .OrderByDescending(item => item.Name);
    }

    private static IEnumerable<TimeSeries> CreateTemperatureChartData(IEnumerable<Temperature> temperatureValues)
    {
        return temperatureValues.GroupBy(item => item.RecordDateTime.Ticks / TimeSpan.FromSeconds(10).Ticks)
            .Select(groupedValues =>
            {
                var firstValue = groupedValues.FirstOrDefault();
                return new TimeSeries()
                {
                    Name = firstValue.RecordDateTime,
                    Value = groupedValues.Average(item => item.Value)
                };
            })
            .ToList();
    }

    protected static IEnumerable<Chart<TimeSeries>> GroupData(Func<Temperature, string> keySelector, PageSetting setting, IList<Temperature> data)
    {
        return data.GroupBy(
            keySelector,
            (key, values) => new Chart<TimeSeries>()
            {
                Name = key,
                Series = CreateTemperatureChartData(values, setting)
            })
            .ToList();
    }

    protected static IEnumerable<Chart<TimeSeries>> GroupData(Func<Temperature, string> keySelector, Func<Temperature, bool> predicate, IList<Temperature> data)
    {
        return data
            .Where(predicate)
            .GroupBy(
                keySelector,
                (key, values) => new Chart<TimeSeries>()
                {
                    Name = key,
                    Series = CreateTemperatureChartData(values)
                })
            .ToList();
    }
}