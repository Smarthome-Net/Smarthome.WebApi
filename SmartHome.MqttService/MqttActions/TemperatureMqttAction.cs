using MQTTnet;
using SmartHome.Common.Collections;
using SmartHome.Common.Models.Db;
using SmartHome.MqttService.ApplicationMessageProcessors;
using SmartHome.MqttService.Observables;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SmartHome.MqttService.MqttActions;

public class TemperatureMqttAction : IMqttAction
{
    private readonly ITemperatureObservable _temperatureObservable;
    private readonly IApplicationMessageProcessor<Temperature> _messageProcessor;
    private bool disposedValue;

    public TemperatureMqttAction(IApplicationMessageProcessor<Temperature> messageProcessor, ITemperatureObservable temperatureObservable)
    {
        _messageProcessor = messageProcessor;
        _temperatureObservable = temperatureObservable;
    }

    public async Task ExecuteAction(MqttApplicationMessage message, string deviceContext, CancellationToken token = default)
    {
        var temperature = await _messageProcessor.ProcessMessage(message, deviceContext, token);
        _temperatureObservable.OnNext(temperature);
    }

    public string GetSensorType()
    {
        return SensorType.Temperature;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _temperatureObservable.Dispose();
            }

            // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // TODO: Große Felder auf NULL setzen
            disposedValue = true;
        }
    }

    // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    // ~TemperatureMqttAction()
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
