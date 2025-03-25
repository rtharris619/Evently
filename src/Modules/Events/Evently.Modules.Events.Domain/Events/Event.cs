using Evently.Modules.Events.Domain.Abstractions;
using Evently.Modules.Events.Domain.Categories;

namespace Evently.Modules.Events.Domain.Events;

public sealed class Event : Entity
{
    // For EF Core to Materialise
    private Event() { }

    public Guid Id { get; private set; }

    public Guid CategoryId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Location { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime? EndsAtUtc { get; private set; }

    public EventStatus Status { get; private set; }

    public static Event Create(
        Category category,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
        {
            throw new ArgumentException("EndsAtUtc must be greater than StartsAtUtc");
        }

        var @event = new Event
        {
            Id = Guid.NewGuid(),
            CategoryId = category.Id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
            Status = EventStatus.Draft
        };

        @event.Raise(new EventCreatedDomainEvent(@event.Id));

        return @event;
    }

    public void Publish()
    {
        if (Status != EventStatus.Draft)
        {
            return;
        }

        Status = EventStatus.Published;

        Raise(new EventPublishedDomainEvent(Id));
    }

    public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        if (StartsAtUtc == startsAtUtc && EndsAtUtc == endsAtUtc)
        {
            return;
        }

        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;

        Raise(new EventRescheduledDomainEvent(Id, startsAtUtc, endsAtUtc));
    }

    public void Cancel(DateTime utcNow)
    {
        if (Status == EventStatus.Cancelled)
        {
            return;
        }

        if (StartsAtUtc < utcNow)
        {
            return;
        }

        Status = EventStatus.Cancelled;

        Raise(new EventCancelledDomainEvent(Id));
    }
}
