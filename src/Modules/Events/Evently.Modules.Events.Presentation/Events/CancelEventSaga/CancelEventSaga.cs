using Automatonymous;
using Evently.Modules.Events.IntegrationEvents;
using Evently.Modules.Ticketing.IntegrationEvents;
using MassTransit;

namespace Evently.Modules.Events.Presentation.Events.CancelEventSaga;

public sealed class CancelEventSaga : MassTransitStateMachine<CancelEventState>
{
    public State CancellationStarted { get; private set; }
    public State PaymentsRefunded { get; private set; }
    public State TicketsArchived { get; private set; }

    public Event<EventCancelledIntegrationEvent> EventCancelled { get; private set; }
    public Event<EventPaymentsRefundedIntegrationEvent> EventPaymentsRefunded { get; private set; }
    public Event<EventTicketsArchivedIntegrationEvent> EventTicketsArchived { get; private set; }
    public Event EventCancellationCompleted { get; private set; }
}
