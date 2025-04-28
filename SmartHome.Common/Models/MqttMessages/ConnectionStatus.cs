namespace SmartHome.Common.Models.MqttMessages;

public enum ConnectionStatus
{
    Unknown = -1, //If connection status cannot be determined or something else is wrong

    Disconnected = 0,
    Connected = 1,
}
