using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using SmartHome.Common.Interfaces;
using System.Threading;
using SmartHome.Common.Exceptions;
using SmartHome.MqttService.JsonConvertes;
using SmartHome.Common.Models.Db;
using SmartHome.Common.Models.MqttMessages;
using System.IO;

namespace SmartHome.MqttService.ApplicationMessageProcessors;

class TemperatureMessageProcessor : IApplicationMessageProcessor<Temperature>
{
    private ILogger<TemperatureMessageProcessor>? _logger;
    private ITemperatureWriterService? _temperatureWriteService;
    private IDeviceService? _deviceService;

    private bool isDisposed;

    public TemperatureMessageProcessor(ILogger<TemperatureMessageProcessor> logger,
        ITemperatureWriterService temperatureWriteService,
        IDeviceService deviceService)
    {
        _logger = logger;
        _temperatureWriteService = temperatureWriteService;
        _deviceService = deviceService;
        SubscriptionTopic = string.Empty;
    }

    public string SubscriptionTopic { get; set; }

    private static JsonSerializerOptions SerializerOptions => new()
    {
        Converters =
        {
            new DateTimeOffsetConverter()
        }
    };

    public async Task<Temperature> ProcessMessage(MqttApplicationMessage applicationMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(TemperatureMessageProcessor));
            }
            using var byteStream = new MemoryStream(applicationMessage.PayloadSegment.ToArray());
            var message = await JsonSerializer.DeserializeAsync<MqttMessage>(byteStream, SerializerOptions, cancellationToken);
            var fullTopic = GetDeviceTopic(applicationMessage.Topic, "temperature");
            var device = await _deviceService!.GetOrCreateDeviceByTopic(fullTopic, cancellationToken);
            var temperature = new Temperature
            {
                RecordDateTime = message!.Time,
                Value = message!.Value,
                DeviceId = device.Id,
                Device = device
            };

            return await _temperatureWriteService!.WriteTemperature(temperature, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationMessageException(ex);
        }
    }

    private string GetDeviceTopic(string topic, string sensorType)
    {
        var baseTopic = SubscriptionTopic.Replace("#", "");
        var sensor = topic.Remove(0, baseTopic.Length);
        var deviceTopic = sensor.Remove(0, sensorType.Length + 1);
        return deviceTopic;
    }

    protected virtual void Dispose(bool disposing, CancellationToken cancellationToken = default)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
            }
            // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
            // TODO: Große Felder auf NULL setzen
            _logger = null;
            _temperatureWriteService = null;
            _deviceService = null;
            isDisposed = true;
        }
    }

    // ~TemperatureMessageProcessor()
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
