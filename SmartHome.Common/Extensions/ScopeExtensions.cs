using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.DTO;
using System;
using System.Collections.Generic;

namespace SmartHome.Common.Extensions;

public static class ScopeExtensions
{
    public static Func<Temperature, string> ToTemperatureKeySelector(this Scope scope) 
    {
        //The default selector is for the room
        Func<Temperature, string> keySelector = item => item.Device.Room;
        switch (scope.ScopeType)
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

    public static Func<Temperature, bool> ToTemperaturePredicate(this Scope scope)
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
