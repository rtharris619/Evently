using Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;
using Evently.Common.Domain;
using Evently.Common.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evently.Common.Presentation.Endpoints;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class ChangeTicketTypePrice : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("ticket-types/{id}/price", async (Guid id, ChangeTicketTypePriceRequest request, ISender sender) =>
        {
            Result result = await sender.Send(new UpdateTicketTypePriceCommand(id, request.Price));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.TicketTypes);
    }

    internal sealed class ChangeTicketTypePriceRequest
    {
        public decimal Price { get; init; }
    }
}
