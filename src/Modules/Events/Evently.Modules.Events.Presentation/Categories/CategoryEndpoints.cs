using Microsoft.AspNetCore.Routing;

namespace Evently.Modules.Events.Presentation.Categories;

public static class CategoryEndpoints
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        ArchiveCategory.MapEndpoint(routeBuilder);
        CreateCategory.MapEndpoint(routeBuilder);
        GetCategories.MapEndpoint(routeBuilder);
        GetCategory.MapEndpoint(routeBuilder);
        UpdateCategory.MapEndpoint(routeBuilder);
    }
}
