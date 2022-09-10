using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public class WeatherHub : Hub<IWeatherHubServerInvoked>, IWeatherHubClientInvoked
{
    private readonly IMediator Mediator;
    public WeatherHub(IMediator mediator) : base()
    {
        Mediator = mediator;
    }

    public async Task SyncState()
    {
        await Clients.Caller.WeatherHasChanged(WeatherHubState.CurrentState);
    }

    public async Task UserChangesWeather(int id)
    {
        WeatherHubState.CurrentState = await Mediator.Send(new GetSingleWeatherForecast(id));
        await Clients.All.WeatherHasChanged(WeatherHubState.CurrentState);
    }
}

internal static class WeatherHubState
{
    internal static RequestResult<WeatherForecastViewModel>? CurrentState { get; set; }
}