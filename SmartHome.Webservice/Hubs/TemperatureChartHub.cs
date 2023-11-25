using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SmartHome.Webservice.Hubs.Interfaces;
using Microsoft.Extensions.Logging;
using SmartHome.Webservice.Helper;

namespace SmartHome.Webservice.Hubs;

public class TemperatureChartHub : Hub<ITemperatureChartHub>
{
    private const string TEMPERATURE_SUBSCRIPTION = "temperature_subscription";
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
        DisposeTemperatureSubscription();
        Context.Items.Clear();
        return base.OnDisconnectedAsync(exception);
    }

    public void Temperature(string scopeFilter) 
    {
        var clients = Clients;
        var context = Context;
        _temperatureHubQueue.SetScope(scopeFilter);
        var subscription = _temperatureHubQueue.TemperaturChartData
           .Where(item => item.ScopeFilter.Equals(scopeFilter))
           .Select(item => item.Chart)
           .Subscribe(chartData =>
            {
                _logger.LogInformation("Charts: {Count}, to Client: {ConnectionId}", chartData.Count(), context.ConnectionId);
                clients.Caller.UpdateTemperatuure(chartData);
            });
        
        if(context.Items.ContainsKey(TEMPERATURE_SUBSCRIPTION))
        {
            DisposeTemperatureSubscription();
        }

        context.Items[TEMPERATURE_SUBSCRIPTION] = subscription;
    }

    private void DisposeTemperatureSubscription()
    {
        if (Context.Items.TryGetValue(TEMPERATURE_SUBSCRIPTION, out var subscription))
        {
            _logger.LogInformation("Dispose");
            var disposable = (IDisposable)subscription;
            disposable.Dispose();
        }
    }
}
