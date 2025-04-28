namespace SmartHome.Common.Models.MqttMessages;

public class DeviceStatus
{
    public ConnectionStatus WifiConnectedStatus { get; set; }
    public ConnectionStatus MqttConnectedStatus { get; set; }
    public float BatteryStatus { get; set; }
    public float CurrentTemperature { get; set; }
}