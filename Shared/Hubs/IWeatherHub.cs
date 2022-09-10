namespace Shared.Hubs;
public interface IWeatherHub
{
    public const string Path = "/weather";
}

public interface IWeatherHubServerInvoked : IWeatherHub
{
    public Func<RequestResult<WeatherForecastViewModel>, Task> WeatherHasChanged { get; }
}

public interface IWeatherHubClientInvoked : IWeatherHub
{
    Task SyncState();
    Task UserChangesWeather(int id);
}
