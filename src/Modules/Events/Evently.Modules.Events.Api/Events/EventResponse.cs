namespace Evently.Modules.Events.Api.Events;

public sealed record EventReponse(
    Guid Id,
    string Title,
    string Description,
    string Location,
    DateTime StartsAtUtc,
    DateTime? EndsAtUtc);
