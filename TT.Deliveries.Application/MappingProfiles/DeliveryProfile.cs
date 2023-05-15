using AutoMapper;
using TT.Deliveries.Application.Commands.CreateDelivery;
using TT.Deliveries.Application.Commands.UpdateDelivery;
using TT.Deliveries.Application.Queries;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.MappingProfiles
{
    public class DeliveryProfile : Profile
    {
        public DeliveryProfile()
        {
            CreateMap<Shared.AccessWindow, Domain.AccessWindow>().ReverseMap();

            CreateMap<Shared.Order, Domain.Order>().ReverseMap();

            CreateMap<Shared.Recipient, Domain.Recipient>().ReverseMap();

            CreateMap<CreateDeliveryCommand, Delivery>()
                .ForMember(d => d.State,
                           op => op.MapFrom(o => MapState(o.State)));

            CreateMap<UpdateDeliveryCommand, Delivery>()
                .ForMember(d => d.State,
                           op => op.MapFrom(o => MapState(o.State)));

            CreateMap<Delivery, DeliveryDetailsDto>().ForMember(d => d.State,
                           op => op.MapFrom(o => MapState(o.State))); ;

        }

        public static DeliveryState MapState(string state)
        {
            DeliveryState deliveryState;
            Enum.TryParse(state, true, out deliveryState);
            return deliveryState;
        }

        public static string MapState(DeliveryState state)
        {
           return Enum.GetName(typeof(DeliveryState), state);
        }
    }
}
