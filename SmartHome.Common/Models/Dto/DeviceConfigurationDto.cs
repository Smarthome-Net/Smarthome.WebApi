namespace SmartHome.Common.Models.Dto;

public class DeviceConfigurationDto
{
    public int MeasureInterval { get; set; }
    public string? MqttHost { get; set; }
    public int MqttPort { get; set; }
    public string? Ssid { get; set; }
    public string? Password { get; set; }
}