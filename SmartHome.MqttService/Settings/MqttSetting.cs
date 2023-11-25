namespace SmartHome.MqttService.Settings;

public class MqttSetting
{
    public BrokerSetting BrokerSetting { get; set; }
    public ClientSetting ClientSetting { get; set; }
    public TopicSetting TopicSetting { get; set; }
}
