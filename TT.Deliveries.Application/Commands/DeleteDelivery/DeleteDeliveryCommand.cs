using MediatR;

namespace TT.Deliveries.Application.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommand : IRequest<Unit>
    {
        public string OrderNumber { get; set; }
    }
}
