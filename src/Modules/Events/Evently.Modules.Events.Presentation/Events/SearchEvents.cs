﻿using Evently.Modules.Events.Application.Events.SearchEvents;
using Evently.Common.Domain;
using Evently.Common.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evently.Common.Presentation.Endpoints;

namespace Evently.Modules.Events.Presentation.Events;

internal sealed class SearchEvents : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("events/search", async (
            ISender sender,
            Guid? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int page = 0,
            int pageSize = 15) =>
        {
            Result<SearchEventsResponse> result = await sender.Send(
                new SearchEventsQuery(categoryId, startDate, endDate, page, pageSize));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.Events);
    }
}
