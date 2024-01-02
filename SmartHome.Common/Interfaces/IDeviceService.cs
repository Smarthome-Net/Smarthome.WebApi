using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Common.Models.Db;

namespace SmartHome.Common.Interfaces;

public  interface IDeviceService
{
    Task<Device> GetDeviceById(string deviceId);
    Task<Device> GetDeviceByTopic(string topic);
    Task<Device> GetOrCreateDeviceByTopic(string topic);
    Task<IEnumerable<Device>> GetRooms();
    Task<IEnumerable<Device>> GetDevices(string room);

    Task<IEnumerable<Device>> GetAllDevices();
    Task<Device> CreateDevice(Device device);
    Task<long> DeleteDevice(string deviceId);
    Task<long> UpdateDevice(Device device);
}
