using Evently.Modules.Events.Application.TicketTypes.CreateTicketType;
using Evently.Common.Domain;
using Evently.Common.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evently.Common.Presentation.Endpoints;

namespace Evently.Modules.Events.Presentation.TicketTypes;

internal sealed class CreateTicketType : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("ticket-types", async (CreateTicketTypeRequest request, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new CreateTicketTypeCommand(
                request.EventId,
                request.Name,
                request.Price,
                request.Currency,
                request.Quantity));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.TicketTypes);
    }

    internal sealed class CreateTicketTypeRequest
    {
        public Guid EventId { get; init; }

        public string Name { get; init; }

        public decimal Price { get; init; }

        public string Currency { get; init; }

        public decimal Quantity { get; init; }
    }
}
