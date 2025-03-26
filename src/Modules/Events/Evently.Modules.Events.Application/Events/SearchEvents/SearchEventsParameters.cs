namespace Evently.Modules.Events.Application.Events.SearchEvents;

internal sealed record SearchEventsParameters(
    int Status,
    Guid? CategoryId,
    DateTime? StartDate,
    DateTime? EndDate,
    int Take,
    int Skip);
