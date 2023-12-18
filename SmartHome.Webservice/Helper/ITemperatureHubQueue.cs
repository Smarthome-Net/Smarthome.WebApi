using SmartHome.Common.Models.DTO;
using System;

namespace SmartHome.Webservice.Helper;

public interface ITemperatureHubQueue
{
    public void SetScope(string scopeValue);

    public IObservable<FilterableChart<TimeSeries>> TemperaturChartData { get; }

}
