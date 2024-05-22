using SmartHome.Common.Extensions;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.MqttService.Observables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace SmartHome.Webservice.Helper;


public class TemperatureHubQueue : ITemperatureHubQueue
{
    private readonly ITemperatureObservable _temperatureObservable;

    private Scope _scope;

    public TemperatureHubQueue(ITemperatureObservable temperatureObservable)
    {
        _temperatureObservable = temperatureObservable;
    }

    public IObservable<IEnumerable<Chart<TimeSeries>>> TemperaturChartData 
    { 
        get => _temperatureObservable.Temperature
            .Buffer(TimeSpan.FromSeconds(2))
            .Where(x => x.Count > 0)
            .Select(CreateTemperatureChart); 
    }

    private IEnumerable<Chart<TimeSeries>> CreateTemperatureChart(IList<Temperature> data)
    {
        var keySelector = _scope.ToTemperatureKeySelector();
        var predictae = _scope.ToPredicate<Temperature>(
            (temp, room) => temp.Device.Room == room, 
            (temperature, room, name) => temperature.Device.Room == room && temperature.Device.Name == name);

        return data
            .Where(predictae)
            .ToTimeSeriesChart(keySelector);
    }

    public void SetScope(Scope scope)
    {
        _scope = scope;
    }
}
