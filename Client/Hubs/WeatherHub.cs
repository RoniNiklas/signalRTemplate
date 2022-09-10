using Microsoft.AspNetCore.SignalR.Client;
using Shared.Hubs;

namespace Client.Hubs;
public class WeatherHub : IWeatherHub, IDisposable
{
    private readonly HubConnection _connection;
    private readonly Action<RequestResult<WeatherForecastViewModel>> _onWeatherChanged;

    public WeatherHub(Action<RequestResult<WeatherForecastViewModel>> onWeatherChanged)
    {
        _onWeatherChanged = onWeatherChanged;
        _connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:5000{IWeatherHub.Path}")
            .Build();

        _connection.StartAsync();
        _connection.On<RequestResult<WeatherForecastViewModel>>(nameof(WeatherHasChanged), WeatherHasChanged);
    }

    public async Task SyncState()
    {
        await _connection.InvokeAsync(nameof(SyncState));
    }

    public async Task UserChangesWeather(int id)
    {
        await _connection.InvokeAsync(nameof(UserChangesWeather), id);
    }

    public async Task WeatherHasChanged(RequestResult<WeatherForecastViewModel> weather)
    {
        _onWeatherChanged.Invoke(weather);
    }

    public void Dispose()
    {
        _connection.DisposeAsync();
    }
}
