using Client.Infra;
using Shared.Hubs;

namespace Client.Hubs;
internal class WeatherHub : ClientHub<IWeatherHubClientInvoked, IWeatherHubServerInvoked>
{
    internal WeatherHub() : base(IWeatherHubServerInvoked.Path)
    {
    }
}
