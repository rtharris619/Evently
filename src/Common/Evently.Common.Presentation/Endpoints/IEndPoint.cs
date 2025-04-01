using Microsoft.AspNetCore.Routing;

namespace Evently.Common.Presentation.Endpoints;

public interface IEndPoint
{
    void MapEndpoint(IEndpointRouteBuilder routeBuilder);
}
