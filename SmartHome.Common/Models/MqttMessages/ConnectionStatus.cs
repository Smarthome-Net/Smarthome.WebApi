namespace SmartHome.Common.Models.MqttMessages;

public enum ConnectionStatus
{
    Unkown = -1, //If connection status cannot be determined or something else is wrong

    Disconnected = 0,
    Connected = 1,
}
