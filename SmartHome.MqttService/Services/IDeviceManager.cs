using SmartHome.Common.Models;
using System.Threading.Tasks;

namespace SmartHome.MqttService.Services;

public interface IDeviceManager
{
    public Task<DeviceStatus?> GetStatus(string deviceId);
        
    public Task<DeviceConfiguration?> GetConfiguration(string deviceId);
    
    public  Task<DeviceConfiguration?> PopulateConfiguration(string deviceId, DeviceConfiguration deviceConfiguration);
}
