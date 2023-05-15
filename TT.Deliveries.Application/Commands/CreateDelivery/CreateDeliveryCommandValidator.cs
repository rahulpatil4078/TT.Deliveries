using FluentValidation;
using TT.Deliveries.Application.Shared;

namespace TT.Deliveries.Application.Commands.CreateDelivery
{
    public class CreateDeliveryCommandValidator : AbstractValidator<CreateDeliveryCommand>
    {
        public CreateDeliveryCommandValidator() 
        {
            RuleFor(p=>p.State).NotEmpty().WithMessage("Delivery State should not be empty");
            RuleFor(p => p.State).IsEnumName(typeof(DeliveryState),false).WithMessage("Delivery state value is not valid");
            RuleFor(r => r.AccessWindow).NotNull();
            RuleFor(r=>r.Order).NotNull();
            RuleFor(t=>t.Recipient).NotNull();
        }
    }
}
