using Evently.Modules.Events.Application.Categories.UpdateCategory;
using Evently.Common.Domain;
using Evently.Common.Presentation.ApiResults;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Evently.Common.Application.Caching;
using Evently.Common.Presentation.Endpoints;

namespace Evently.Modules.Events.Presentation.Categories;

internal sealed class UpdateCategory : IEndPoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("categories/{id}", async (Guid id, UpdateCategoryRequest request, ISender sender, ICacheService cacheService) =>
        {
            Result result = await sender.Send(new UpdateCategoryCommand(id, request.Name));

            if (result.IsSuccess)
            {
                await cacheService.RemoveAsync(CacheKeys.Categories);
            }

            return result.Match(() => Results.Ok(), ApiResults.Problem);
        })
        .WithTags(Tags.Categories);
    }

    internal sealed class UpdateCategoryRequest
    {
        public string Name { get; init; }
    }
}
