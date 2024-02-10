using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Common.Models.Db;

namespace SmartHome.Common.Interfaces;

public  interface IDeviceService
{
    Task<Device> GetDeviceById(string deviceId, CancellationToken cancellationToken = default);
    Task<Device> GetDeviceByTopic(string topic, CancellationToken cancellationToken = default);
    Task<Device> GetOrCreateDeviceByTopic(string topic, CancellationToken cancellationToken = default);
    Task<IEnumerable<Device>> GetRooms(CancellationToken cancellationToken = default);
    Task<IEnumerable<Device>> GetDevices(string room, CancellationToken cancellationToken = default);

    Task<IEnumerable<Device>> GetAllDevices(CancellationToken cancellationToken = default);
    Task<Device> CreateDevice(Device device, CancellationToken cancellationToken = default);
    Task<long> DeleteDevice(string deviceId, CancellationToken cancellationToken = default);
    Task<long> UpdateDevice(Device device, CancellationToken cancellationToken = default);
}
