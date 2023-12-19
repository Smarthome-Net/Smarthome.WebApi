using SmartHome.Common.Models.DTO;
using SmartHome.Common.Models.DTO.Charts;
using System;

namespace SmartHome.Webservice.Helper;

public interface ITemperatureHubQueue
{
    public void SetScope(string scopeValue);

    public IObservable<FilterableChart<TimeSeries>> TemperaturChartData { get; }

}
