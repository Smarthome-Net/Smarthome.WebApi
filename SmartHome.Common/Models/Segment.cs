using SmartHome.Common.Models.DTO;

namespace SmartHome.Common.Models;

public readonly struct Segment 
{
    public Segment(string value)
    {
        Value = value;
    }

    public string Value { get; }
}