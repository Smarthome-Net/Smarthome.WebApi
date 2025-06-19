namespace SmartHome.Common.Models;

public record Segment(string Value) 
{
    public static implicit operator Segment(string Value) => new(Value);
}