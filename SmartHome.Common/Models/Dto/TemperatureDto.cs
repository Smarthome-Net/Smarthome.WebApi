using System;

namespace SmartHome.Common.Models.Dto;

public class TemperatureDto
{
    public required string Id { get; set; }
    public DateTimeOffset RecordDateTime { get; set; }
    public float Value { get; set; }
    public DeviceDto? Device { get; set; }
}
