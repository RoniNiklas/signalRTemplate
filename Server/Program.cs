using Handlers.WeatherForecast;
using Microsoft.AspNetCore.ResponseCompression;
using Server.Hubs;
using Server.Infra.MediatR;

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

// SIGNALR
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddSignalR();

// APP
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseHttpsRedirection();
app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.MapHub<WeatherHub>(IWeatherHub.Path);

app.Run();
