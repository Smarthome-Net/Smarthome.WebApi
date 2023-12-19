using System.Collections.Generic;
using SmartHome.Common.Models.DTO.Charts;
using SmartHome.Common.Models.DTO.Requests;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureReaderService
{
    IEnumerable<Chart<TimeSeries>> GetTemperature(TemperatureRequest request);
}
