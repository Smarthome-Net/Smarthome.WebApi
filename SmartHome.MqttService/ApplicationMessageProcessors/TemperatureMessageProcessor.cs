using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MQTTnet;
using SmartHome.Common.Interfaces;
using System.Threading;
using SmartHome.Common.Exceptions;
using SmartHome.MqttService.JsonConvertes;
using SmartHome.Common.Models.MqttMessages;
using System.IO;
using SmartHome.Common.Extensions.Mapping;
using SmartHome.Common.Models.Db;

namespace SmartHome.MqttService.ApplicationMessageProcessors;

public class TemperatureMessageProcessor : IApplicationMessageProcessor<Common.Models.Dto.TemperatureDto>
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

    public async Task<Common.Models.Dto.TemperatureDto> ProcessMessage(MqttApplicationMessage applicationMessage, string deviceContext, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger!.LogInformation("Start processing new application message");
            ObjectDisposedException.ThrowIf(_isDisposed, typeof(TemperatureMessageProcessor));
            
            using var byteStream = new MemoryStream([.. applicationMessage.PayloadSegment]);
            var message = await JsonSerializer.DeserializeAsync<MqttMessage>(byteStream, SerializerOptions, cancellationToken);
            var device = await _deviceService!.GetOrCreateDeviceByTopic(deviceContext, cancellationToken);
            var data = new Temperature
            {
                RecordDateTime = message!.Time,
                Value = message.Value,
                DeviceId = device.Id,
            };

            await _temperatureWriteService!.WriteTemperature(data, cancellationToken);
            var temperature = data.ToDto();
            temperature.Device = device.ToDto();
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
