namespace Evently.Modules.Events.Application.Events.GetEvents;

public sealed record EventsResponse(
    Guid Id,
    Guid CategoryId,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc);
