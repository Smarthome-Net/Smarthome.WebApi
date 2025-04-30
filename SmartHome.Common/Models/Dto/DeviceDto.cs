using SmartHome.Common.Models.MqttMessages;

namespace SmartHome.Common.Models.Dto;

public class DeviceDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Room { get; set; }
    public string? Topic { get; set; }
    public DeviceConfigurationDto? Configuration { get; set; }
}
