using FluentValidation;

namespace Evently.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

internal sealed class UpdateTicketTypePriceValidator : AbstractValidator<UpdateTicketTypePriceCommand>
{
    public UpdateTicketTypePriceValidator()
    {
        RuleFor(x => x.TicketTypeId).NotEmpty();
        RuleFor(x => x.Price).GreaterThan(decimal.Zero);
    }
}
