using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using System;
using System.Collections.Generic;

namespace SmartHome.Webservice.Helper;

public interface ITemperatureHubQueue
{
    public void SetScope(Scope scope);

    public IObservable<IEnumerable<Chart<TimeSeries>>> TemperaturChartData { get; }

}
