using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Events;

public static class EventEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        CancelEvent.MapEndpoint(routeBuilder);
        CreateEvent.MapEndpoint(routeBuilder);
        GetEvent.MapEndpoint(routeBuilder);
        GetEvents.MapEndpoint(routeBuilder);
        PublishEvent.MapEndpoint(routeBuilder);
        RescheduleEvent.MapEndpoint(routeBuilder);
        SearchEvents.MapEndpoint(routeBuilder);
    }
}
