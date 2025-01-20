
using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Api.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Api;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        CreateEvent.MapEndPoint(routeBuilder);
        GetEvent.MapEndpoint(routeBuilder);
    }

    public static IServiceCollection AddEventsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        string dbConnectionString = configuration.GetConnectionString("Database");

        services.AddDbContext<EventsDbContext>(options =>
            options
                .UseNpgsql(
                    dbConnectionString,
                    npgsqlOptions => npgsqlOptions
                        .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events))
                .UseSnakeCaseNamingConvention()
        );

        return services;
    }
}
