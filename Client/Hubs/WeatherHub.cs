using Microsoft.AspNetCore.SignalR.Client;
using Shared.Hubs;
using System.Linq.Expressions;
using System.Reflection;

namespace Client.Hubs;
public class WeatherHub : ClientHub<IWeatherHubClientInvoked, IWeatherHubServerInvoked>, IWeatherHubServerInvoked, IDisposable
{
    private readonly Action<RequestResult<WeatherForecastViewModel>?> _onWeatherChanged;
    public WeatherHub(Action<RequestResult<WeatherForecastViewModel>?> onWeatherChanged) : base()
    {
        _onWeatherChanged = onWeatherChanged;
        _connection.On<RequestResult<WeatherForecastViewModel>>("WeatherHasChanged", WeatherHasChanged);
    }

    public async Task SyncState()
    {
        await InvokeAsync(x => x.SyncState);
    }

    public async Task UserChangesWeather(int id)
    {
        await InvokeAsync(x => x.UserChangesWeather, id);
    }

    public async Task WeatherHasChanged(RequestResult<WeatherForecastViewModel>? weather)
    {
        _onWeatherChanged.Invoke(weather);
    }

}

public static class HubExtensions
{
    public static IDisposable Register<TNameSource>(
        this HubConnection connection,
        Expression<Func<TNameSource, Func<Task>>> boundMethod,
        Func<Task> handler)
        => connection.On(_GetMethodName(boundMethod), handler);

    public static IDisposable Register<TNameSource, TIn>(
        this HubConnection connection,
        Expression<Func<TNameSource, Func<TIn, Task>>> boundMethod,
        Action<TIn> handler)
        => connection.On(_GetMethodName(boundMethod), handler);

    public static IDisposable Register<TNameSource, TIn1, TIn2>(
        this HubConnection connection,
        Expression<Func<TNameSource, Func<TIn1, TIn2, Task>>> boundMethod,
        Action<TIn1, TIn2> handler)
        => connection.On(_GetMethodName(boundMethod), handler);


    private static string _GetMethodName<T>(Expression<T> boundMethod)
    {
        var unaryExpression = (UnaryExpression)boundMethod.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
        var methodInfoExpression = (ConstantExpression)methodCallExpression.Object;
        var methodInfo = (MethodInfo)methodInfoExpression.Value;
        return methodInfo.Name;
    }
}
public abstract class ClientHub<TClientInvoked, TServerInvoked> : IDisposable
{
    protected readonly HubConnection _connection;
    public ClientHub()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:5000{IWeatherHub.Path}")
            .Build();

        _connection.StartAsync();
    }

    public Task InvokeAsync(
        Expression<Func<TClientInvoked, Func<Task>>> boundMethod)
        => _connection.InvokeAsync(_GetMethodName(boundMethod));

    public Task InvokeAsync<TIn>(
        Expression<Func<TClientInvoked, Func<TIn, Task>>> boundMethod,
        TIn input)
        => _connection.InvokeAsync(_GetMethodName(boundMethod), input);

    public Task InvokeAsync<TIn1, TIn2>(
        Expression<Func<TClientInvoked, Func<TIn1, TIn2, Task>>> boundMethod,
        TIn1 input1, TIn2 input2)
        => _connection.InvokeAsync(_GetMethodName(boundMethod), input1, input2);

    public IDisposable Register(
        Expression<Func<TServerInvoked, Func<Task>>> boundMethod,
        Func<Task> handler)
        => _connection.On(_GetMethodName(boundMethod), handler);

    public IDisposable Register<TIn>(
        Expression<Func<TServerInvoked, Func<TIn, Task>>> boundMethod,
        Func<TIn, Task> handler)
        => _connection.On(_GetMethodName(boundMethod), handler);

    public IDisposable Register<TIn1, TIn2>(
        Expression<Func<TServerInvoked, Func<TIn1, TIn2, Task>>> boundMethod,
        Func<TIn1, TIn2, Task> handler)
        => _connection.On(_GetMethodName(boundMethod), handler);

    private static string _GetMethodName<T>(Expression<T> boundMethod)
    {
        var unaryExpression = (UnaryExpression)boundMethod.Body;
        var methodCallExpression = (MethodCallExpression)unaryExpression.Operand;
        var methodInfoExpression = (ConstantExpression)methodCallExpression.Object;
        var methodInfo = (MethodInfo)methodInfoExpression.Value;
        return methodInfo.Name;
    }
    public void Dispose()
    {
        _connection.DisposeAsync();
    }
}
