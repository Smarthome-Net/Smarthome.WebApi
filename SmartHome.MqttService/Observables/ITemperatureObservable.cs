using SmartHome.Common.Models.Dto;
using System;

namespace SmartHome.MqttService.Observables;

public interface ITemperatureObservable
{

    public IObservable<TemperatureDto> Temperature { get; }

    public void OnNext(TemperatureDto temperature);
}