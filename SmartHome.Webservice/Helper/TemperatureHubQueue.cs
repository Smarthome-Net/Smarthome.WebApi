using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.DTO;
using SmartHome.Common.QueryHelper;
using SmartHome.MqttService.Providers;
using SmartHome.MqttService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SmartHome.Webservice.Helper;


public class TemperatureHubQueue : TemperatureQueryBase, ITemperatureHubQueue
{
    private readonly IMqttClientService _mqttClientService;
    private readonly Subject<FilterableChart<TimeSeries>> _subject;

    private string _scopeValue;
    private ScopeType _scope;

    public TemperatureHubQueue(MqttClientServiceProvider mqttClientServiceProvider)
    {
        _mqttClientService = mqttClientServiceProvider.MqttClientService;
        _subject = new Subject<FilterableChart<TimeSeries>>();
        _mqttClientService.Temperature
            .Subscribe(data => 
            {
                Process(new List<Temperature> { data });
            });
    }


    private void Process(IList<Temperature> rawData)
    {
        var data = CreateTemperatureChart(rawData);
        var chartData = new FilterableChart<TimeSeries>(_scopeValue, data);
        _subject.OnNext(chartData);
    }


    public IObservable<FilterableChart<TimeSeries>> TemperaturChartData { get => _subject.AsObservable(); }

    public IEnumerable<Chart<TimeSeries>> CreateTemperatureChart(IList<Temperature> data)
    {
        var keySelector = CreateKeySelector(_scope);

        return GroupData(keySelector, data);
    }

    private static ScopeType GetScope(string scopeValue)
    {
        if (scopeValue.Equals(string.Empty))
        {
            return ScopeType.All;
        }

        if(scopeValue.Contains('/'))
        {
            return ScopeType.Room;
        }
        return ScopeType.Device;
    }

    public void SetScope(string scopeValue)
    {
        _scopeValue = scopeValue;
        _scope = GetScope(scopeValue);
    }
}
