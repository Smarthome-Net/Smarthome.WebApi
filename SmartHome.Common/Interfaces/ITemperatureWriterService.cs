using System.Threading;
using System.Threading.Tasks;
using SmartHome.Common.Models.Db;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureWriterService
{
    
    Task<Temperature> WriteTemperature(Temperature temperature, CancellationToken cancellationToken = default);
}
