using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using SmartHome.Common.Models.Dto.Charts;

namespace SmartHome.Webservice.Hubs.Interfaces;

public interface ITemperatureChartHub
{
    Task UpdateTemperature([NoEnumeration] IEnumerable<Chart<DateTimeOffset, float>> chartData);
}
