using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.TicketTypes;

public static class TicketTypeEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        ChangeTicketTypePrice.MapEndpoint(routeBuilder);
        CreateTicketType.MapEndpoint(routeBuilder);
        GetTicketType.MapEndpoint(routeBuilder);
        GetTicketTypes.MapEndpoint(routeBuilder);
    }
}
