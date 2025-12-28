using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SmartHome.Webservice.Hubs.Interfaces;
using Microsoft.Extensions.Logging;
using SmartHome.Webservice.Helper;
using SmartHome.Common.Models.Dto;

namespace SmartHome.Webservice.Hubs;

public class TemperatureChartHub : Hub<ITemperatureChartHub>
{
    private const string TemperatureSubscription = "temperature_subscription";
    private readonly ITemperatureHubQueue _temperatureHubQueue;
    private readonly ILogger<TemperatureChartHub> _logger;

    public TemperatureChartHub(ITemperatureHubQueue temperatureHubQueue, ILogger<TemperatureChartHub> logger)
    {
        _temperatureHubQueue = temperatureHubQueue;
        _logger = logger;
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        _logger.LogInformation("Disconnection of client: {ConnectionId}", Context.ConnectionId);
        TryDisposeSubscription();
        Context.Items.Clear();
        return base.OnDisconnectedAsync(exception);
    }

    public void Temperature(Scope scope)
    {
        var context = Context;
        var clients = Clients;
        _temperatureHubQueue.SetScope(scope);
        var subscription = _temperatureHubQueue
            .GetTemperaturChartData()
            .Subscribe(chartData =>
            {
                _logger.LogInformation("Sending {Count} charts to client: {ConnectionId}", chartData.Count(), context.ConnectionId);
                clients.Caller.UpdateTemperature(chartData);
            });

        TryDisposeSubscription(); //Try to clean up the old subscription
        _logger.LogInformation("New subscription of client: {ConeectionId} with scope value: {Value}", context.ConnectionId, scope.Value);
        Context.Items[TemperatureSubscription] = subscription;
    }

    private void TryDisposeSubscription()
    {
        if (!Context.Items.TryGetValue(TemperatureSubscription, out var subscription))
        {
            //only appears for first connection of the client
            _logger.LogInformation("No subscription available to dispose");
            return;
        }

        if (subscription is IDisposable disposable)
        {
            disposable?.Dispose();
            _logger.LogInformation("Dispose subscription of client: {ConnectionId}", Context.ConnectionId);
            return;
        }
        
        // should be possible, but if this was in the logs something goes wrong
        _logger.LogInformation("Unable to Dispose subcription of client: {ConnectionId}, no IDisosable object found", Context.ConnectionId);
    }
}
