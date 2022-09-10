namespace Shared.Requests;
public record GetSingleWeatherForecast(int Id) : IRequest<RequestResult<WeatherForecastViewModel>>
{
    public ValidationResult Validate()
    {
        return new Validator().Validate(this);
    }

    public class Validator : AbstractValidator<GetSingleWeatherForecast>
    {
        public Validator()
        {
            RuleFor(item => item.Id).GreaterThan(0);
        }
    }
}
