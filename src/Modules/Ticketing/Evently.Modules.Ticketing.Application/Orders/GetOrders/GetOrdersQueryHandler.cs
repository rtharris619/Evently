using System.Data.Common;
using Dapper;
using Evently.Common.Application.Data;
using Evently.Common.Application.Messaging;
using Evently.Common.Domain;

namespace Evently.Modules.Ticketing.Application.Orders.GetOrders;

internal sealed class GetOrdersQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetOrdersQuery, IReadOnlyCollection<OrdersResponse>>
{
    public async Task<Result<IReadOnlyCollection<OrdersResponse>>> Handle(
        GetOrdersQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(OrdersResponse.Id)},
                 customer_id AS {nameof(OrdersResponse.CustomerId)},
                 status AS {nameof(OrdersResponse.Status)},
                 total_price AS {nameof(OrdersResponse.TotalPrice)},
                 created_at_utc AS {nameof(OrdersResponse.CreatedAtUtc)}
             FROM ticketing.orders
             WHERE customer_id = @CustomerId
             """;

        List<OrdersResponse> orders = (await connection.QueryAsync<OrdersResponse>(sql, request)).AsList();

        return orders;
    }
}
