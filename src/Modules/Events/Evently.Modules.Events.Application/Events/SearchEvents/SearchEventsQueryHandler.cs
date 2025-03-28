using System.Data.Common;
using Dapper;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Application.Abstractions.Messaging;
using Evently.Modules.Events.Application.Events.GetEvents;
using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Events;

namespace Evently.Modules.Events.Application.Events.SearchEvents;

internal sealed class SearchEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchEventsQuery, SearchEventsResponse>
{
    public async Task<Result<SearchEventsResponse>> Handle(SearchEventsQuery request, CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        var parameters = new SearchEventsParameters
        (
           Status: (int)EventStatus.Published,
           CategoryId: request.CategoryId,
           StartDate: request.StartDate?.Date,
           EndDate: request.EndDate?.Date,
           Take: request.PageSize,
           Skip: (request.Page - 1) * request.PageSize
        );

        IReadOnlyCollection<EventsResponse> events = await GetEventsAsync(connection, parameters);

        int totalCount = await CountEventsAsync(connection, parameters);

        return new SearchEventsResponse(request.Page, request.PageSize, totalCount, events);
    }

    private static async Task<IReadOnlyCollection<EventsResponse>> GetEventsAsync(
        DbConnection connection,
        SearchEventsParameters parameters)
    {
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
                WHERE
                   status = @Status AND
                   (@CategoryId IS NULL OR category_id = @CategoryId) AND
                   (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
                   (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
                ORDER BY starts_at_utc
                OFFSET @Skip
                LIMIT @Take
            """;

        List<EventsResponse> events = (await connection.QueryAsync<EventsResponse>(sql, parameters)).AsList();

        return events;
    }

    private static async Task<int> CountEventsAsync(
        DbConnection connection,
        SearchEventsParameters parameters)
    {
        const string sql =
            """
            SELECT COUNT(*)
            FROM events.events
            WHERE
               status = @Status AND
               (@CategoryId IS NULL OR category_id = @CategoryId) AND
               (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
               (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
            """;

        int totalCount = await connection.ExecuteScalarAsync<int>(sql, parameters);

        return totalCount;
    }
}
