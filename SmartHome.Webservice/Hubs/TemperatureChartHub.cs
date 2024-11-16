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
        _logger.LogInformation("Disconnection");
        TryDisposeSubscription();
        Context.Items.Clear();
        return base.OnDisconnectedAsync(exception);
    }

    public void Temperature(Scope scope) 
    {
        _temperatureHubQueue.SetScope(scope);
        var subscription = _temperatureHubQueue
            .GetTemperaturChartData()
            .Subscribe(chartData =>
            {
                _logger.LogInformation("Charts: {Count}, to Client: {ConnectionId}", chartData.Count(), Context.ConnectionId);
                Clients.Caller.UpdateTemperature(chartData);
            });
        
        TryDisposeSubscription(); //Try to clean up the old subscription
        Context.Items[TemperatureSubscription] = subscription;
    }

    private void TryDisposeSubscription()
    {
        if (!Context.Items.TryGetValue(TemperatureSubscription, out var subscription))
        {
            return;
        }

        _logger.LogInformation("Dispose");
        var disposable = (IDisposable)subscription;
        disposable?.Dispose();
    }
}
