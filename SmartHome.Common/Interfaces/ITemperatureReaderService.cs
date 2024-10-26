using System.Collections.Generic;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Requests;

namespace SmartHome.Common.Interfaces;

public interface ITemperatureReaderService
{
    IEnumerable<TemperatureDto> GetTemperature(TemperatureRequest request);
}
