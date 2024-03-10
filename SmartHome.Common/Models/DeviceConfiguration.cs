namespace SmartHome.Common.Models;

public class DeviceConfiguration
{
    public string Name { get; set; }
    public string Room { get; set; }
    public int Interval { get; set; }
    public string MqttHost { get; set; }
    public int MqttPort { get; set;}
    public string Ssid { get; set; }
    public string SsidPassword { get; set; }
}
