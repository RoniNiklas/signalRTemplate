namespace Shared.Hubs;
internal interface SignalREndpointDefinition<TIn> : SignalREndpointDefinition
{
    Func<TIn> Handle();
}
internal interface SignalREndpointDefinition
{
}

