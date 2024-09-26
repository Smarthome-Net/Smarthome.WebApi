namespace SmartHome.MqttService.Settings;

public class MqttSetting
{
    public required BrokerSetting BrokerSetting { get; set; }
    public required ClientSetting ClientSetting { get; set; }
    public required TopicSetting TopicSetting { get; set; }
}
