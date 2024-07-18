using MongoDB.Driver;
using SmartHome.Common.Models.Db;

namespace SmartHome.Tests;

public class DeviceTestCurser : IAsyncCursor<Device>
{
    public IEnumerable<Device> Current => [
        new Device 
        { 
            Id = "1",
            Name = "device",
            Room = "test",
            Topic = "test2/device"
        },
        new Device 
        { 
            Id = "1",
            Name = "device",
            Room = "test",
            Topic = "test/device"
        }
    ];

    public void Dispose() { }

    public bool MoveNext(CancellationToken cancellationToken = default)
    {
        return true;
    }

    public Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(true);
    }
}
