namespace Shared.Requests;

public record GetWeatherForecasts : IRequest<RequestResult<WeatherForecastViewModel[]>>
{
}
