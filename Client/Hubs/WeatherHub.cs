using Microsoft.AspNetCore.SignalR.Client;
using Shared.Hubs;
using System.Reflection;

namespace Client.Hubs;
public class WeatherHub : IWeatherHubClientInvoked, IWeatherHubServerInvoked, IDisposable
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
        _connection.Register(WeatherHasChanged);
    }

    public async Task SyncState()
    {
        await _connection.InvokeAsync(nameof(SyncState));
    }

    public async Task UserChangesWeather(int id)
    {
        await _connection.InvokeAsync(nameof(UserChangesWeather), id);
    }

    public Func<RequestResult<WeatherForecastViewModel>, Task> WeatherHasChanged =>
        async (RequestResult<WeatherForecastViewModel> weather) =>
        {
            _onWeatherChanged.Invoke(weather);
        };

    public void Dispose()
    {
        _connection.DisposeAsync();
    }
}

public static class HubExtensions
{

    public static IDisposable Register<T>(this HubConnection connection, Func<T, Task> handler)
    {
        Console.WriteLine(handler.GetMethodInfo().Name);
        return connection.On(nameof(handler), handler);
    }
}
public abstract class Hub<T>
{
    private readonly HubConnection _connection;
    public Hub()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:5000{IWeatherHub.Path}")
            .Build();

        _connection.StartAsync();
    }

}
