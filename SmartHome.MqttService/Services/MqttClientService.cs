using MQTTnet;
using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Client;
using Microsoft.Extensions.Logging;
using SmartHome.MqttService.Settings;
using System.Reactive.Subjects;
using SmartHome.Common.Exceptions;
using System.Reactive.Linq;
using SmartHome.MqttService.Providers;
using SmartHome.Common.Models.Db;
using MQTTnet.Extensions.Rpc;

namespace SmartHome.MqttService.Services;

public class MqttClientService : IMqttClientService
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;
    private readonly ILogger<MqttClientService> _logger;
    private readonly MqttSetting _mqttSetting;
    private readonly IApplicationMessageProvider _applicationMessageProvider;
    private readonly Subject<Temperature> _temperatureSubject;

    private readonly MqttFactory _mqttFactory;
    private bool _isDisposed = false;

    public MqttClientService(ILogger<MqttClientService> logger, 
        MqttClientOptions options,
        MqttSetting mqttSetting,
        IApplicationMessageProvider applicationMessageProvider)
    {
        _logger = logger;
        _options = options;
        _mqttSetting = mqttSetting;
        _applicationMessageProvider = applicationMessageProvider;
        _temperatureSubject = new Subject<Temperature>();

        _mqttFactory = new MqttFactory();
        _client = _mqttFactory.CreateMqttClient();
        _client.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
        _client.ConnectedAsync += HandleConnectedAsync;
        _client.DisconnectedAsync += HandleDisconnectedAsync;
    }

    public IObservable<Temperature> Temperature { get => _temperatureSubject.AsObservable(); }

    #region IHostedService Implementation
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _client.ConnectAsync(_options, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to Connect MQTT Broker. {ex.Message}");
        }
       
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            var disconnectOption = new MqttClientDisconnectOptions
            {
                Reason = MqttClientDisconnectOptionsReason.NormalDisconnection,
                ReasonString = "Normal Disconect"
            };
            await _client.DisconnectAsync(disconnectOption, cancellationToken);
        }
    }
    #endregion

    #region IMqttClientConnectedHandler, IMqttClientDisconnectedHandler, IMqttApplicationMessageReceivedHandler Implementation

    public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        var source = new CancellationTokenSource();
        if (eventArgs.ApplicationMessage.Topic.Contains("temperature"))
        {
            try
            {
                using var messageProcessor = _applicationMessageProvider.GetApplicationMessageProcessor<Temperature>();
                messageProcessor.SubscriptionTopic = _mqttSetting?.TopicSetting?.SubscriptionTopic!;
                var temperature = await messageProcessor.ProcessMessage(eventArgs.ApplicationMessage, source.Token);
                _temperatureSubject.OnNext(temperature);
            }
            catch (ApplicationMessageException ex)
            {
                _logger.LogError("{Message} \r\n {StackTrace}", ex.Message, ex.StackTrace);
                source.Cancel();
            }
            
        }
    }

    public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
    {
        MqttFactory factory = new();

        var subscribeOptions = factory.CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic(_mqttSetting?.TopicSetting?.SubscriptionTopic!))
            .Build();


        _logger.LogInformation("Connected to Mqtt Broker");
        await _client.SubscribeAsync(subscribeOptions);
    }

    public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
    {
        _logger.LogInformation("Disconnected from Mqtt Broker: {Reason}", eventArgs.Reason);
        if(!_client.IsConnected && eventArgs.Reason == MqttClientDisconnectReason.NormalDisconnection) 
        {
            await _client.ConnectAsync(_options);
        }
    }

    public IMqttRpcClient CreateMqttRpcClient() 
    {
        var options = new MqttRpcClientOptionsBuilder()
            .WithTopicGenerationStrategy(new SmarthomeRpcTopicGenerationStrategy(_mqttSetting))
            .Build();
        
        return _mqttFactory.CreateMqttRpcClient(_client, options);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _temperatureSubject.Dispose();
                _client.Dispose();
            }

            _isDisposed = true;
        }
    }

    // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
    // ~MqttClientService()
    // {
    //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
