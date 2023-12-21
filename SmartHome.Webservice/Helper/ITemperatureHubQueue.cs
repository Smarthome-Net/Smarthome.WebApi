using SmartHome.Common.Models.DTO;
using SmartHome.Common.Models.DTO.Charts;
using System;
using System.Collections.Generic;

namespace SmartHome.Webservice.Helper;

public interface ITemperatureHubQueue
{
    public void SetScope(Scope scope);

    public IObservable<IEnumerable<Chart<TimeSeries>>> TemperaturChartData { get; }

}
