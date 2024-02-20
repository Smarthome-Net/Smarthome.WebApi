using System.Collections.Generic;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.Models.Dto.Requests;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureReaderService
{
    IEnumerable<Chart<TimeSeries>> GetTemperature(TemperatureRequest request);
}
