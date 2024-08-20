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

public class TemperatureMessageProcessor : IApplicationMessageProcessor<Temperature>
{
    private ILogger<TemperatureMessageProcessor>? _logger;
    private ITemperatureWriterService? _temperatureWriteService;
    private IDeviceService? _deviceService;

    private bool _isDisposed;

    public TemperatureMessageProcessor(ILogger<TemperatureMessageProcessor> logger,
        ITemperatureWriterService temperatureWriteService,
        IDeviceService deviceService)
    {
        _logger = logger;
        _temperatureWriteService = temperatureWriteService;
        _deviceService = deviceService;
    }

    private static JsonSerializerOptions SerializerOptions => new()
    {
        Converters =
        {
            new DateTimeOffsetConverter()
        },
        PropertyNameCaseInsensitive = true,
    };

    public async Task<Temperature> ProcessMessage(MqttApplicationMessage applicationMessage, string deviceContext, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger!.LogInformation("Start processing new application message");
            ObjectDisposedException.ThrowIf(_isDisposed, typeof(TemperatureMessageProcessor));
            
            using var byteStream = new MemoryStream([.. applicationMessage.PayloadSegment]);
            var message = await JsonSerializer.DeserializeAsync<MqttMessage>(byteStream, SerializerOptions, cancellationToken);
            var device = await _deviceService!.GetOrCreateDeviceByTopic(deviceContext, cancellationToken);
            var temperature = new Temperature
            {
                RecordDateTime = message!.Time,
                Value = message!.Value,
                DeviceId = device.Id,
            };

            await _temperatureWriteService!.WriteTemperature(temperature, cancellationToken);
            temperature.Device = device;
            return temperature;
        }
        catch (Exception ex)
        {
            _logger!.LogError("Processing application failed with: {Message}", ex.Message);
            throw new ApplicationMessageException(ex);
        }
    }

    protected virtual void Dispose(bool disposing, CancellationToken cancellationToken = default)
    {
        if (!_isDisposed)
        {
            _logger = null;
            _temperatureWriteService = null;
            _deviceService = null;
            _isDisposed = true;
        }
    }

    // ~TemperatureMessageProcessor()
    // {
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
