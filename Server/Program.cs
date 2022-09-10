using Handlers.WeatherForecast;
using Server.Infra.MediatR;
using Shared.Requests;

// SERVICES
var builder = WebApplication.CreateBuilder(args);

// SWAGGER
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining(typeof(GetWeatherForecasts));

// MEDIATR
builder.Services.AddMediatR(typeof(GetWeatherForecastsQueryHandler));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000", "http://localhost:3001")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// APP
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.InstallEndpoints(typeof(GetWeatherForecasts), typeof(GetWeatherForecastsQueryHandler));

// just for benchmark comparison
app.MapPost("WeatherForecasts", async (IMediator mediator, GetWeatherForecasts input) => await mediator.Send(input));

app.Run();
