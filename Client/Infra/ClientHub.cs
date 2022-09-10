using Microsoft.AspNetCore.SignalR.Client;
using System.Linq.Expressions;
using System.Reflection;

namespace Client.Infra;

internal abstract class ClientHub<TClientInvoked, TServerInvoked> : IDisposable
{
    protected readonly HubConnection _connection;
    internal ClientHub(string path)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl($"https://localhost:5000{path}")
            .Build();

        _connection.StartAsync();
    }

    // INVOKERS
    internal Task InvokeAsync(
        Expression<Func<TClientInvoked, Func<Task>>> boundMethod)
        => _connection.InvokeAsync(_GetMethodName(boundMethod));

    internal Task InvokeAsync<TIn>(
        Expression<Func<TClientInvoked, Func<TIn, Task>>> boundMethod,
        TIn input)
        => _connection.InvokeAsync(_GetMethodName(boundMethod), input);

    internal Task InvokeAsync<TIn1, TIn2>(
        Expression<Func<TClientInvoked, Func<TIn1, TIn2, Task>>> boundMethod,
        TIn1 input1, TIn2 input2)
        => _connection.InvokeAsync(_GetMethodName(boundMethod), input1, input2);

    // CONSUMERS
    internal IDisposable Register(
        Expression<Func<TServerInvoked, Func<Task>>> boundMethod,
        Func<Task> handler)
        => _connection.On(_GetMethodName(boundMethod), handler);

    internal IDisposable Register<TIn>(
        Expression<Func<TServerInvoked, Func<TIn, Task>>> boundMethod,
        Func<TIn, Task> handler)
        => _connection.On(_GetMethodName(boundMethod), handler);

    internal IDisposable Register<TIn1, TIn2>(
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

    void IDisposable.Dispose()
    {
        _connection.DisposeAsync();
    }
}