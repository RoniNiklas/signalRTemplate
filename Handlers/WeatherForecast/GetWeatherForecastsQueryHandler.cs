namespace Handlers.WeatherForecast;
public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecasts, RequestResult<WeatherForecastViewModel[]>>
{
    private static readonly string[] _summaries = new[]
    {
                "Freezing",
                "Bracing",
                "Chilly",
                "Cool",
                "Mild",
                "Warm",
                "Balmy",
                "Hot",
                "Sweltering",
                "Scorching"
            };
    public async Task<RequestResult<WeatherForecastViewModel[]>> Handle(GetWeatherForecasts request, CancellationToken cancellationToken)
    {
        return Enumerable.Range(1, 5).Select(index =>
                new WeatherForecastViewModel
                (
                    DateTime.Now.AddDays(index),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                ))
                .ToArray();
    }
}
