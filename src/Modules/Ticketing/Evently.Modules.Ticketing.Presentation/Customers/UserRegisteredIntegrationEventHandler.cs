using Evently.Modules.Ticketing.Application.Customers.CreateCustomer;
using Evently.Modules.Ticketing.Domain.Customers;
using System.Threading;
using Evently.Modules.Users.IntegrationEvents;
using MassTransit;
using MediatR;
using Evently.Common.Domain;
using Evently.Common.Application.Exceptions;
using Evently.Common.Application.EventBus;

namespace Evently.Modules.Ticketing.Presentation.Customers;

internal sealed class UserRegisteredIntegrationEventHandler(ISender sender)
    : IntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public override async Task Handle(UserRegisteredIntegrationEvent integrationEvent, CancellationToken cancellationToken = default)
    {
        Result result = await sender.Send(
            new CreateCustomerCommand(
                integrationEvent.UserId,
                integrationEvent.Email,
                integrationEvent.FirstName,
                integrationEvent.LastName), 
            cancellationToken);

        if (result.IsFailure)
        {
            throw new EventlyException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
