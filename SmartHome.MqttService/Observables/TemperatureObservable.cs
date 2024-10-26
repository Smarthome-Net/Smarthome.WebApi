using SmartHome.Common.Models.Dto;
using System;
using System.Reactive.Subjects;

namespace SmartHome.MqttService.Observables;

public class TemperatureObservable : ITemperatureObservable
{
    private readonly Subject<TemperatureDto> _temperatureSubject;

    public TemperatureObservable()
    {
        _temperatureSubject = new Subject<TemperatureDto>();
    }

    public IObservable<TemperatureDto> Temperature => _temperatureSubject;

    public void OnNext(TemperatureDto temperature)
    {
        _temperatureSubject.OnNext(temperature);
    }
}
