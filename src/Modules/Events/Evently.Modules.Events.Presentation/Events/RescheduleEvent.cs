﻿using Evently.Modules.Events.Application.Events.RescheduleEvent;
using Evently.Common.Domain;
using Evently.Common.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evently.Common.Presentation.Endpoints;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class RescheduleEvent : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("events/{id}/reschedule", async (Guid id, RescheduleEventRequest request, ISender sender) =>
        {
            Result result = await sender.Send(
                new RescheduleEventCommand(id, request.StartsAtUtc, request.EndsAtUtc));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.Events);
    }

    internal sealed class RescheduleEventRequest
    {
        public DateTime StartsAtUtc { get; init; }
        public DateTime? EndsAtUtc { get; init; }
    }
}
