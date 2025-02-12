using SmartHome.Common.Collections;

namespace SmartHome.Common.Models.Dto.Charts;

/// <summary>
/// Strong typed chart class with generic series collection
/// </summary>
/// <typeparam name="TName"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class Chart<TName, TValue>
{
    public required string Name { get; set; }
    public Series<TName, TValue> Series { get; set; } = [];
}
