using FluentValidation;
using TT.Deliveries.Application.Shared;

namespace TT.Deliveries.Application.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommandValidator : AbstractValidator<UpdateDeliveryCommand>
    {
        public UpdateDeliveryCommandValidator() 
        {
            RuleFor(p => p.State).NotEmpty().WithMessage("Delivery State should not be empty");
            RuleFor(p => p.State).IsEnumName(typeof(DeliveryState), false).WithMessage("Delivery state value is not valid");
            RuleFor(r => r.Order).NotNull();
            RuleFor(r => r.Order.OrderNumber).NotEmpty();          
        }
    }
}
