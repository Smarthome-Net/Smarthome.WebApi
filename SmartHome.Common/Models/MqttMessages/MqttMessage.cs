using System;

namespace SmartHome.Common.Models.MqttMessages;

public class MqttMessage
{
    public float Value { get; set; }
    public DateTimeOffset Time { get; set; }
}
