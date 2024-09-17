using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using System;

namespace SmartHome.Common.Extensions;

public static class ScopeExtensions
{
    public static Func<Temperature, string> ToTemperatureKeySelector(this Scope scope) 
    {
        return scope.ScopeType switch
        {
            ScopeType.All => item => item.Device!.Room!,
            ScopeType.Room or ScopeType.Device => item => item.Device!.Name!,
            _ => throw new InvalidOperationException($"Scope was invaild: {scope}"),
        };
    }

    public static Func<Device, bool> ToDevicePredicate(this Scope scope)
    {
        return scope.ScopeType switch
        {
            ScopeType.All => item => true,
            ScopeType.Room => item => item.Topic!.StartsWith(scope.Value!),
            ScopeType.Device => item => string.Equals(item.Topic, scope.Value),
            _ => throw new InvalidOperationException($"Scope was invaild: {scope}"),//Invalid enum value for scope
        };
    }
}
