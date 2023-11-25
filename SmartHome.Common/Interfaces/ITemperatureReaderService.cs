using System.Collections.Generic;
using SmartHome.Common.Models.DTO;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureReaderService
{
    IEnumerable<Chart> GetTemperature(TemperatureRequest request);
}
