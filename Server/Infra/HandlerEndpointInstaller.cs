namespace Server.Infra;

internal static class HandlerEndpointInstaller
{
    internal static void InstallEndpoints(this WebApplication app, Type requestAssemblyMarker, Type handlerAssemblyMarker)
    {
        var requestTypes = requestAssemblyMarker
            .Assembly
            .GetTypes()
            .Where(t => t.GetInterface(typeof(IRequest<>).Name) != null)
            .ToList();

        var handlerTypes = handlerAssemblyMarker
            .Assembly
            .GetTypes()
            .Where(t => t.GetInterface(typeof(IRequestHandler<,>).Name) != null)
            .ToList();

        var handledRequestTypes = handlerTypes
            .SelectMany(type => type.GetInterfaces()
                                        .Select(i => i.GenericTypeArguments[0]));

        foreach (var requestType in requestTypes)
        {

            if (!handledRequestTypes.Contains(requestType))
            {
                throw new Exception($"RequestType {requestType.Name} is not handled. Please implement a handler in the handler assembly.");
            }

            var returnType = requestType.GetInterface(typeof(IRequest<>).Name)?.GenericTypeArguments[0];

            if (returnType is null)
            {
                throw new Exception($"ReturnType of {requestType.Name} could not be determined from its IRequest interface. All requests must implement IRequest<T> for the T to be discoverable.");
            }

            app.MapPost(requestType.Name, async (IMediator mediator, HttpRequest httpRequest) =>
                {
                    var request = await httpRequest.ReadFromJsonAsync(requestType);
                    var res = await mediator.Send(request);
                    return res;
                })
                .Accepts(requestType, false, "application/json")
                .Produces(200, returnType); ;
        }
    }
}
