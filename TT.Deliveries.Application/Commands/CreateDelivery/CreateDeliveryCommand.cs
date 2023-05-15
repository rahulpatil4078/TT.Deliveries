using MediatR;
using TT.Deliveries.Application.Shared;

namespace TT.Deliveries.Application.Commands.CreateDelivery
{
    public class CreateDeliveryCommand : BaseDeliveryEntity ,  IRequest<Guid> 
    {

    }
}
