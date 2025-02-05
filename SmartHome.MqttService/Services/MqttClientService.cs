using System;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet.Client;
using Microsoft.Extensions.Logging;
using SmartHome.MqttService.Settings;
using SmartHome.Common.Exceptions;
using MQTTnet.Extensions.Rpc;
using Microsoft.Extensions.Options;
using SmartHome.MqttService.Extensions;
using SmartHome.MqttService.Providers;

namespace SmartHome.MqttService.Services;

public class MqttClientService : IMqttClientService
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _clientOptions;
    private readonly ILogger<MqttClientService> _logger;
    private readonly MqttSetting _mqttSetting;
    private readonly MqttFactoryProvider _mqttFactoryProvider;
    private readonly MqttActionProvider _actionProvider;
    private bool _isDisposed;

    public MqttClientService(ILogger<MqttClientService> logger,
        MqttClientOptions clientOptions,
        IOptions<MqttOptions> mqttOptions,
        MqttActionProvider actionProvider,
        MqttFactoryProvider mqttFactoryProvider)
    {
        _logger = logger;
        _clientOptions = clientOptions;
        _actionProvider = actionProvider;
        _mqttSetting = mqttOptions.Value.MqttSetting;
        _mqttFactoryProvider = mqttFactoryProvider;
        
        _client = _mqttFactoryProvider().CreateMqttClient();
        _client.ApplicationMessageReceivedAsync += HandleApplicationMessageReceivedAsync;
        _client.ConnectedAsync += HandleConnectedAsync;
        _client.DisconnectedAsync += HandleDisconnectedAsync;
    }

    #region IHostedService Implementation
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting MQTT client");
        try
        {
            await _client.ConnectAsync(_clientOptions, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable to Connect MQTT Broker. {ex.Message}");
        }
       
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping MQTT client");
        var disconnectOption = new MqttClientDisconnectOptions
        {
            Reason = MqttClientDisconnectOptionsReason.NormalDisconnection,
        };
        await _client.DisconnectAsync(disconnectOption, cancellationToken);
    }
    #endregion

    #region Mqtt Actions Handlers
    private async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
    {
        _logger.LogInformation("Received notification from MQTT broker.");
        if(IsRpcTopic(eventArgs.ApplicationMessage.Topic)) 
        {
            return;
        }
        
        var source = new CancellationTokenSource();
        var action = _actionProvider(eventArgs.ApplicationMessage.Topic);
        try
        {
            _logger.LogInformation("Execute MQTT action for {SensorType}", action!.SensorType);
            await action.ProcessAction(eventArgs.ApplicationMessage, source.Token);
        }
        catch (ApplicationMessageException ex)
        {
            _logger.LogError("{Message} \r\n {StackTrace}", ex.Message, ex.StackTrace);
            await source.CancelAsync();
        }
    }
    
    private async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
    {
        var subscribeOptions = _mqttFactoryProvider().CreateSubscribeOptionsBuilder()
            .WithTopicFilter(f => f.WithTopic($"{_mqttSetting.TopicSetting.SubscriptionTopic}/#"))
            .Build();

        _logger.LogInformation("Connected to Mqtt Broker");
        await _client.SubscribeAsync(subscribeOptions);
    }

    private async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
    {
        _logger.LogInformation("Disconnected from Mqtt Broker: {Reason}", eventArgs.Reason);
        if(!_client.IsConnected && eventArgs.Reason != MqttClientDisconnectReason.NormalDisconnection) 
        {
            _logger.LogInformation("Reconnect to Mqtt Broker");
            await _client.ConnectAsync(_clientOptions);
        }
    }
    #endregion

    #region IMqttClientService Implementations
    public IMqttRpcClient CreateMqttRpcClient() 
    {
        var options = new MqttRpcClientOptionsBuilder()
            .WithTopicGenerationStrategy(new SmartHomeRpcTopicGenerationStrategy(_mqttSetting))
            .Build();
        
        return _mqttFactoryProvider().CreateMqttRpcClient(_client, options);
    }
    #endregion
    
    private bool IsRpcTopic(string topic)
    {
        return topic.StartsWith(_mqttSetting.TopicSetting.SubscriptionRpcTopic);
    }

    private void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }
        
        if (disposing)
        {
            _client.Dispose();
        }

        _isDisposed = true;
    }
    
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
