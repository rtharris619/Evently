﻿
using FluentValidation;

namespace Evently.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();
        RuleFor(x => x.Description)
           .NotEmpty();
        RuleFor(x => x.Location)
           .NotEmpty();
        RuleFor(x => x.StartsAtUtc)
           .NotEmpty();

        RuleFor(x => x.EndsAtUtc)
            .Must((cmd, endsAtUtc) => endsAtUtc > cmd.StartsAtUtc)
            .When(x => x.EndsAtUtc.HasValue);
    }
}
