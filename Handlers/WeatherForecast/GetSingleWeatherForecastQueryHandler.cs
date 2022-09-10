namespace Handlers.WeatherForecast;

internal class GetSingleWeatherForecastQueryHandler : IRequestHandler<GetSingleWeatherForecast, RequestResult<WeatherForecastViewModel>>
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

    public async Task<RequestResult<WeatherForecastViewModel>> Handle(GetSingleWeatherForecast request, CancellationToken cancellationToken)
    {
        return new WeatherForecastViewModel
                (
                    DateTime.Now.AddDays(request.Id),
                    Random.Shared.Next(-20, 55),
                    _summaries[Random.Shared.Next(_summaries.Length)]
                );
    }
}
