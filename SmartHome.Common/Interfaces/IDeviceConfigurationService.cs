using System.Threading.Tasks;
using SmartHome.Common.Models;

namespace SmartHome.Common.Interfaces;

interface IDeviceConfigurationService
{
    Task<int> UpdateDeviceConfigurationService(DeviceConfiguration configuration);
}
