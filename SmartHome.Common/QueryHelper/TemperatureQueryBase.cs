using MongoDB.Driver;
using MongoDB.Driver.Linq;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.DTO;
using SmartHome.Common.Models.DTO.Charts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartHome.Common.QueryHelper;

public abstract class TemperatureQueryBase
{
    protected static Func<Temperature, string> CreateKeySelector(ScopeType scope)
    {
        //The default selector is for the room
        Func<Temperature, string> keySelector = item => item.Device.Room;
        switch (scope)
        {
            case ScopeType.All:
                break;
            case ScopeType.Room:
            case ScopeType.Device:
                //If the scope is room we use the device name for the selector, because we only want to display
                //the data for the selected room
                keySelector = item => item.Device.Name;
                break;
            default:
                //Invalid enum value for scope
                throw new InvalidOperationException($"Scope was invaild: {scope}");
        }
        return keySelector;
    }

    protected static Func<Temperature, bool> CreatePredicate(Scope scope)
    {
        Func<Temperature, bool> predicate = item => true;
        switch (scope.ScopeType)
        {
            case ScopeType.All:
                break;
            case ScopeType.Room:
                predicate = item => item.Device.Room == scope.Value;
                break;
            case ScopeType.Device:
                //Layout of scopeValue in this block should be MyRoom/Window. MyRoom is the room where the device stands
                //and the Window is the name of the device. In this case, the number of results should be exactly 2 
                if (TrySplitString(scope.Value, '/', out List<string> results))
                {
                    if (results.Count == 2)
                    {
                        predicate = item => item.Device.Room == results[0] && item.Device.Name == results[1];
                    }
                    else
                    {
                        predicate = item => item.Device.Room == results[0];
                    }
                }
                break;
            default:
                //Invalid enum value for scope
                throw new InvalidOperationException($"Scope was invaild: {scope}");
        }
        return predicate;
    }

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
            .OrderBy(item => item.Name);
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

    protected static IEnumerable<Chart<TimeSeries>> GroupData(Func<Temperature, string> keySelector, IList<Temperature> data)
    {
        return data.GroupBy(
            keySelector,
            (key, values) => new Chart<TimeSeries>()
            {
                Name = key,
                Series = CreateTemperatureChartData(values)
            })
            .ToList();
    }

    private static bool TrySplitString<TReturn>(string stringToSplit, char seperator, out List<TReturn> outList)
    {
        //make sure the list is initialized
        outList = new();

        var splitedString = stringToSplit.Split(seperator);
        foreach (var item in splitedString)
        {
            try
            {
                TReturn value = (TReturn)Convert.ChangeType(item, typeof(TReturn));
                outList.Add(value);
            }
            catch
            {
                outList = null;
                return false;
            }
        }
        return true;
    }
}