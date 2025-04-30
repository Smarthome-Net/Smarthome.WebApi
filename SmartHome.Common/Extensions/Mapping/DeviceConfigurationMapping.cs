using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.MqttMessages;

namespace SmartHome.Common.Extensions.Mapping;

public static class DeviceConfigurationMapping
{
    public static DeviceConfigurationDto ToDto(this DeviceConfiguration configuration)
    {
        return new DeviceConfigurationDto
        {
            Ssid = configuration!.Ssid,
            Password = configuration.Password,
            MqttHost = configuration.MqttHost,
            MqttPort = configuration.MqttPort,
            MeasureInterval = configuration.MeasureInterval,
        };
    }

    public static DeviceConfiguration ToMessage(this DeviceConfigurationDto dto, DeviceDto device)
    {
        return new DeviceConfiguration
        {
            Ssid = dto.Ssid,
            Password = dto.Password,
            MqttHost = dto.MqttHost,
            MqttPort = dto.MqttPort,
            MeasureInterval = dto.MeasureInterval,
            Room = device.Room,
            DeviceName = device.Room
        };
    }
}