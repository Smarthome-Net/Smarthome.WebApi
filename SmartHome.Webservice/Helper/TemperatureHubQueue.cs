using SmartHome.Common.Extensions;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.Dto;
using SmartHome.Common.Models.Dto.Charts;
using SmartHome.Common.QueryHelper;
using SmartHome.MqttService.Providers;
using SmartHome.MqttService.Services;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace SmartHome.Webservice.Helper;


public class TemperatureHubQueue : TemperatureQueryBase, ITemperatureHubQueue
{
    private readonly IMqttClientService _mqttClientService;

    private Scope _scope;

    public TemperatureHubQueue(MqttClientServiceProvider mqttClientServiceProvider)
    {
        _mqttClientService = mqttClientServiceProvider.MqttClientService;
    }

    public IObservable<IEnumerable<Chart<TimeSeries>>> TemperaturChartData 
    { 
        get => _mqttClientService.Temperature
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

        return GroupData(keySelector, predictae, data);
    }

    public void SetScope(Scope scope)
    {
        _scope = scope;
    }
}
