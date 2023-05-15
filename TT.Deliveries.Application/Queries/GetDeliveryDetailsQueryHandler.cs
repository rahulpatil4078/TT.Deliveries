using AutoMapper;
using MediatR;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Exceptions;
using TT.Deliveries.Application.Logging;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.Queries
{
    public class GetDeliveryDetailsQueryHandler : IRequestHandler<GetDeliveryDetailsQuery, List<DeliveryDetailsDto>>
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IAppLogger<GetDeliveryDetailsQueryHandler> _logger;
        public GetDeliveryDetailsQueryHandler(IMapper mapper, IDeliveryRepository deliveryRepository,
               IAppLogger<GetDeliveryDetailsQueryHandler> logger)
        {
            _mapper = mapper;
            _deliveryRepository = deliveryRepository;
            _logger = logger;
        }
        public async Task<List<DeliveryDetailsDto>> Handle(GetDeliveryDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (!Enum.TryParse(request.State, true, out DeliveryState deliveryState))
                {
                    _logger.LogError($"Invalid Delivery State received in request");
                    throw new BadRequestException($"Invalid Delivery State received in request");                
                }

                var deliveries = await _deliveryRepository.GetByStateAsync(deliveryState);

                var data = _mapper.Map<List<DeliveryDetailsDto>>(deliveries);

                _logger.LogInformation($"Delivery details successfully retrieved for delivery state {request.State}");

                return data;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception {ex.Message} occured for Get Request for delivery state {request.State}");
                throw;
            }

        }
    }
}
