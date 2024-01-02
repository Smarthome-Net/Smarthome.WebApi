namespace SmartHome.Common.Models;

public enum ConnectionStatus 
{
    Unkown = -1, //If connection status cannot be determined or something else is wrong
    
    Disconnected = 0,
    Connected = 1,
}

public class DeviceStatus
{
    public ConnectionStatus WifiConnectionStatus { get; set; }
    public ConnectionStatus MqttConnectionStatus { get; set; }
    public float BatteryStatus { get; set; }
    public float LastTemperature { get; set; }
}