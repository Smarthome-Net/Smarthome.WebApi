using System.Threading.Tasks;
using SmartHome.Common.Models.MqttMessages;

namespace SmartHome.Common.Interfaces;

interface IDeviceConfigurationService
{
    Task<int> UpdateDeviceConfigurationService(DeviceConfiguration configuration);
}
