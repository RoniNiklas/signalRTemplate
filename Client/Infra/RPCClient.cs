using MediatR;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Client.Infra;

internal static class RPCClient
{
    internal const string BaseUrl = "https://localhost:5000/";
    private static HttpClient Client => new HttpClient()
    {
        BaseAddress = new Uri(BaseUrl)
    };

    internal async static Task<TReturn> HandleAsync<TReturn>(IRequest<TReturn> request)
    {
        var jsonContent = JsonSerializer.Serialize(request, request.GetType());
        var res = await Client.PostAsync(request.GetType().Name, new StringContent(jsonContent, Encoding.UTF8, "application/json"));
        return (await res.Content.ReadFromJsonAsync<TReturn>())!;
    }
}
