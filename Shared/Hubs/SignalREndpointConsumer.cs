namespace Shared.Hubs;
internal interface SignalREndpointConsumer
{
    List<SignalREndpointDefinition> Endpoints { get; set; }
    void Register();
}
