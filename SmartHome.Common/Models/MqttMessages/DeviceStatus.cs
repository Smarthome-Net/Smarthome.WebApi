namespace SmartHome.Common.Models.MqttMessages;

public class DeviceStatus
{
    public ConnectionStatus WifiConnectionStatus { get; set; }
    public ConnectionStatus MqttConnectionStatus { get; set; }
    public float BatteryStatus { get; set; }
    public float LastTemperature { get; set; }
}