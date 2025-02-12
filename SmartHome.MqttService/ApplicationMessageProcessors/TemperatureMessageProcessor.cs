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
    private ITemperatureService? _temperatureService;
    private IDeviceService? _deviceService;

    private bool _isDisposed;

    public TemperatureMessageProcessor(ILogger<TemperatureMessageProcessor> logger,
        ITemperatureService temperatureWriteService,
        IDeviceService deviceService)
    {
        _logger = logger;
        _temperatureService = temperatureWriteService;
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
            ObjectDisposedException.ThrowIf(_isDisposed, typeof(TemperatureMessageProcessor));
            _logger!.LogInformation("Start processing new application message");
            
            using var byteStream = new MemoryStream([.. applicationMessage.PayloadSegment]);
            var message = await JsonSerializer.DeserializeAsync<MqttMessage>(byteStream, SerializerOptions, cancellationToken);
            var device = await _deviceService!.GetOrCreateDeviceByTopic(deviceContext, cancellationToken);
            var temperature = new Temperature
            {
                RecordDateTime = message!.Time,
                Value = message.Value,
                DeviceId = device.Id,
            };

            await _temperatureService!.CreateTemperature(temperature, cancellationToken);
            _logger!.LogInformation("Finished processing new application message");
            return temperature.ToDto(device);
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
            _temperatureService = null;
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
