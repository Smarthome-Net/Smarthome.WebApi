using SmartHome.Common.Models.MqttMessages;
using System.Threading.Tasks;

namespace SmartHome.Common.Interfaces;

public interface IDeviceManager
{
    public Task<DeviceStatus?> GetStatus(string deviceId);

    public Task<DeviceConfiguration?> GetConfiguration(string deviceId);

    public Task<DeviceConfiguration?> PopulateConfiguration(string deviceId, DeviceConfiguration deviceConfiguration);
}
