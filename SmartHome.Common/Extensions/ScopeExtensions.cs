using SmartHome.Common.Collections;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using System;

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

    public static Func<TType, bool> ToPredicate<TType>(this Scope scope,
        Func<TType, string, bool> roomSelector,
        Func<TType, string, string, bool> deviceSelector)
    {
        Func<TType, bool> predicate = item => true;
        switch (scope.ScopeType)
        {
            case ScopeType.All:
                break;
            case ScopeType.Room:
                //we don't realy need to split the value, we just assume that the value only contain the room name
                predicate = item => roomSelector(item, scope.Value);
                break;
            case ScopeType.Device:
                //The scope value contains the for the room and the device seperated by the '/', for example: myRoom/Window
                //This new extenions method split the value into a strong typed collection, to improve the handling with each segment
                var segments = scope.SplitValueIntoSegments();

                if (segments.Count == 1)
                {
                    predicate = item => roomSelector(item, segments[0].Value);
                }

                if (segments.Count == 2)
                {
                    predicate = item => deviceSelector(item, segments[0].Value, segments[1].Value);
                }
                break;
            default:
                //Invalid enum value for scope
                throw new InvalidOperationException($"Scope was invaild: {scope}");
        }
        return predicate;
    }

    private static ScopeValueSegment SplitValueIntoSegments(this Scope scope) 
    {
        var result = new ScopeValueSegment();
        foreach (var value in scope.Value.Split(ScopeValueSegment.SegmentSeperator))
        {
            result.AddSegment(value);
        }
        return result;
    }
}
