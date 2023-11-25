namespace SmartHome.Common.Models;

public class DeviceConfiguration
{
    public string IP { get; set; }
    public string DNS { get; set; }
    public string Subnet { get; set; }
    public bool Dhcp { get; set; }
    public string MqttTopic { get; set; }
    public string Name { get; set; }

    public string MqttServer { get; set; }
    public string MqttServerPassword {get; set;}
    public string MqttServerCertificateKey { get; set; }

    public string WlanSsid { get; set; }
    public string WlanPassword { get; set; }
    public string WlanCertificateKey { get; set; }
}
