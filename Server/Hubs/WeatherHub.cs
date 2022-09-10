using Microsoft.AspNetCore.SignalR;

namespace Server.Hubs;

public class WeatherHub : Hub, IWeatherHub
{
    private readonly IMediator Mediator;
    public WeatherHub(IMediator mediator) : base()
    {
        Mediator = mediator;
    }

    public async Task SyncState()
    {
        await Clients.Caller.SendAsync(nameof(WeatherHasChanged), WeatherHubState.CurrentState);
    }

    public async Task UserChangesWeather(int id)
    {
        WeatherHubState.CurrentState = await Mediator.Send(new GetSingleWeatherForecast(id));
        await WeatherHasChanged(WeatherHubState.CurrentState);
    }

    public async Task WeatherHasChanged(RequestResult<WeatherForecastViewModel> weather)
    {
        await Clients.All.SendAsync(
            nameof(WeatherHasChanged),
            weather
        );
    }
}

internal static class WeatherHubState
{
    internal static RequestResult<WeatherForecastViewModel>? CurrentState { get; set; }
}