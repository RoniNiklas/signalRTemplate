// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Shared.DTOs;
using System.Net.Http.Json;

var res = await new HttpClient().PostAsJsonAsync(APIBenchMark.assemblyscannedUrl, new { });
res.EnsureSuccessStatusCode();
var data = await res.Content.ReadFromJsonAsync<WeatherForecastViewModel[]>();
if (data is null || data.Length == 0)
{
    throw new Exception("");
}

res = await new HttpClient().PostAsJsonAsync(APIBenchMark.hardCodedUrl, new { });
res.EnsureSuccessStatusCode();
data = await res.Content.ReadFromJsonAsync<WeatherForecastViewModel[]>();
if (data is null || data.Length == 0)
{
    throw new Exception("");
}

BenchmarkRunner.Run<APIBenchMark>();

public class APIBenchMark
{
    public const string assemblyscannedUrl = "https://localhost:5000/GetWeatherForecastsQuery";
    public const string hardCodedUrl = "https://localhost:5000/WeatherForecasts";
    readonly HttpClient client = new();

    [Benchmark]
    public async Task GetAssemblyScanned()
    {
        var res = await client.PostAsJsonAsync(assemblyscannedUrl, new { });
    }

    [Benchmark]
    public async Task GetHardCoded()
    {
        var res = await client.PostAsJsonAsync(hardCodedUrl, new { });
    }
}