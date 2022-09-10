using System.Text.Json.Serialization;

namespace Shared.DTOs;
public record RequestResult<TResponse> : RequestResult
{
    public TResponse? Data { get; init; }

    public RequestResult(TResponse data) => Data = data;

    [JsonConstructor]
    public RequestResult() { }

    public void Switch(Action<TResponse> f0, Action<ValidationError> f1)
    {
        if (Data is not null)
        {
            f0(Data);
            return;
        }
        if (ValidationError is not null)
        {
            f1(ValidationError);
            return;
        }
    }

    public TResult Match<TResult>(Func<TResponse, TResult> f0, Func<ValidationError, TResult> f1)
    {
        if (Data is not null)
        {
            return f0(Data);
        }

        if (ValidationError is not null)
        {
            return f1(ValidationError);
        }

        throw new ArgumentException($"Neither a {typeof(TResponse).Name} or {nameof(ValidationError)} was provided for the Match-method");
    }

    public static implicit operator RequestResult<TResponse>(TResponse t) => new(t);
}

public record RequestResult
{
    public ValidationError? ValidationError { get; init; }
    public RequestResult(ValidationError error) => ValidationError = error;
    [JsonConstructor]
    public RequestResult() { }

    public static RequestResult<T> Success<T>(T data) => new(data);
    public static RequestResult Invalid(ValidationError error) => new(error);

    public static implicit operator RequestResult(ValidationError error) => new(error);
}
