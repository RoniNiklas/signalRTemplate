namespace Shared.Hubs;
public interface IWeatherHub
{
    public const string Path = "/weather";
    Task SyncState();
    Task UserChangesWeather(int id);
    Task WeatherHasChanged(RequestResult<WeatherForecastViewModel> weather);
}
