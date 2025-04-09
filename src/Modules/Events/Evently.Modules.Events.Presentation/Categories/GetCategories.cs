using Evently.Modules.Events.Application.Categories.GetCategories;
using Evently.Modules.Events.Application.Categories.GetCategory;
using Evently.Common.Domain;
using Evently.Common.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evently.Common.Application.Caching;
using Evently.Common.Presentation.Endpoints;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class GetCategories : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("categories", async (ISender sender, ICacheService cacheService) =>
        {
            IReadOnlyCollection<CategoryResponse> categoryResponses = await
                cacheService.GetAsync<IReadOnlyCollection<CategoryResponse>>(CacheKeys.Categories);

            if (categoryResponses is not null)
            {
                return Results.Ok(categoryResponses);
            }

            Result<IReadOnlyCollection<CategoryResponse>> result = await sender.Send(new GetCategoriesQuery());

            if (result.IsSuccess)
            {
                await cacheService.SetAsync(CacheKeys.Categories, result.Value);
            }

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .RequireAuthorization()
        .WithTags(Tags.Categories);
    }
}
