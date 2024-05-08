using SmartHome.Common.Models.Db;
using System;

namespace SmartHome.MqttService.Observables;

public interface ITemperatureObservable
{

    public IObservable<Temperature> Temperature { get; }

    public void OnNext(Temperature temperature);
}