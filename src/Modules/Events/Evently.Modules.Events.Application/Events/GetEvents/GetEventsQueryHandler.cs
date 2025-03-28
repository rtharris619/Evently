using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;

namespace Evently.Modules.Events.Application.Events.GetEvents;

internal sealed class GetEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetEventsQuery, IReadOnlyCollection<EventsResponse>>
{
    public async Task<Result<IReadOnlyCollection<EventsResponse>>> Handle(
        GetEventsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(EventsResponse.Id)},
                 category_id AS {nameof(EventsResponse.CategoryId)},
                 title AS {nameof(EventsResponse.Title)},
                 description AS {nameof(EventsResponse.Description)},
                 location AS {nameof(EventsResponse.Location)},
                 starts_at_utc AS {nameof(EventsResponse.StartsAtUtc)},
                 ends_at_utc AS {nameof(EventsResponse.EndsAtUtc)}
             FROM events.events
             """;

        List<EventsResponse> events = (await connection.QueryAsync<EventsResponse>(sql, request)).AsList();

        return events;
    }
}
