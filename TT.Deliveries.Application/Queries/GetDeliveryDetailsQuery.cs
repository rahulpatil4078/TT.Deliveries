using MediatR;

namespace TT.Deliveries.Application.Queries
{
    public record GetDeliveryDetailsQuery(string State) : IRequest<List<DeliveryDetailsDto>>;
   
}
