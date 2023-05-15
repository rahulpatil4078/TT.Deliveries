using MediatR;
using TT.Deliveries.Application.Shared;

namespace TT.Deliveries.Application.Commands.UpdateDelivery
{
    public class UpdateDeliveryCommand : BaseDeliveryEntity ,  IRequest<Unit> 
    {

    }
}
