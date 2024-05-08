using SmartHome.Common.Models.Db;
using System;
using System.Reactive.Subjects;

namespace SmartHome.MqttService.Observables;

public class TemperatureObservable : ITemperatureObservable
{
    private readonly Subject<Temperature> _temperatureSubject;

    public TemperatureObservable()
    {
        _temperatureSubject = new Subject<Temperature>();
    }

    public IObservable<Temperature> Temperature => _temperatureSubject;

    public void OnNext(Temperature temperature)
    {
        _temperatureSubject.OnNext(temperature);
    }
}
