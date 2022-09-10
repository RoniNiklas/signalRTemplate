namespace Shared.Hubs;

public interface IWeatherHubServerInvoked
{
    public const string Path = "/weather";
    Task WeatherHasChanged(RequestResult<WeatherForecastViewModel> weather);
}

public interface IWeatherHubClientInvoked
{
    Task SyncState();
    Task UserChangesWeather(int id);
}
