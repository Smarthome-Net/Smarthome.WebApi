using System.Collections.Generic;

namespace SmartHome.Common.Models.Dto.Charts;

/// <summary>
/// Strong typed chart class with generic series collection
/// </summary>
/// <typeparam name="TName"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class Chart<TName, TValue>
{
    public required string Name { get; set; }
    public IEnumerable<Series<TName, TValue>> Series { get; set; } = [];
}
