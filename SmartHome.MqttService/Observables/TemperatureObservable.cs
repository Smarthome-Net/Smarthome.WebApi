using SmartHome.Common.Models.Db;
using System;
using System.Reactive.Subjects;

namespace SmartHome.MqttService.Observables;

public class TemperatureObservable : ITemperatureObservable
{
    private readonly Subject<Temperature> _temperatureSubject;
    private bool disposedValue;

    public TemperatureObservable()
    {
        _temperatureSubject = new Subject<Temperature>();
    }

    public IObservable<Temperature> Temperature => _temperatureSubject;

    public void OnNext(Temperature temperature)
    {
        _temperatureSubject.OnNext(temperature);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _temperatureSubject.Dispose();
            }

            // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // TODO: Große Felder auf NULL setzen
            disposedValue = true;
        }
    }

    // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    // ~TemperatureObservable()
    // {
    //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
