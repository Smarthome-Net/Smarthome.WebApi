using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureService
{
    IEnumerable<TemperatureDto> GetTemperature(Scope? scope);

    Task CreateTemperature(Temperature temperature, CancellationToken cancellationToken = default);
}
