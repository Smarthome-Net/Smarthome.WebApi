using System.Collections.Generic;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureReaderService
{
    IEnumerable<TemperatureDto> GetTemperature(Scope? scope);
}
