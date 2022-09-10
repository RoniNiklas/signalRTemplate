namespace Shared.Hubs;
public interface IWeatherHub
{
    public const string Path = "/weather";
}

public interface IWeatherHubServerInvoked : IWeatherHub
{
    Task WeatherHasChanged(RequestResult<WeatherForecastViewModel> weather);
}

public interface IWeatherHubClientInvoked : IWeatherHub
{
    Task SyncState();
    Task UserChangesWeather(int id);
}
