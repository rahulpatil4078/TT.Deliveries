using AutoMapper;
using MediatR;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Exceptions;
using TT.Deliveries.Application.Logging;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.Commands.CreateDelivery
{
    public class CreateDeliveryCommandHandler : IRequestHandler<CreateDeliveryCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IAppLogger<CreateDeliveryCommandHandler> _logger;

        public CreateDeliveryCommandHandler(IMapper mapper, IDeliveryRepository deliveryRepository,
             IAppLogger<CreateDeliveryCommandHandler> logger) 
        {
            _mapper = mapper;
            _deliveryRepository = deliveryRepository;
            _logger = logger;
        }

       public async Task<Guid> Handle(CreateDeliveryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateDeliveryCommandValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (validationResult.Errors.Any())
                {
                    _logger.LogError($"Invalid Delivery Creation Request for order number {request.Order.OrderNumber}");
                    throw new BadRequestException("Invalid Delivery Creation Request", validationResult);
                }

                var deliveryToCreate = _mapper.Map<Delivery>(request);

                deliveryToCreate.Id = Guid.NewGuid();

                await _deliveryRepository.CreateAsync(deliveryToCreate);

                _logger.LogInformation($"Delivery created successfully for OrderNumber {request.Order.OrderNumber}");

                return deliveryToCreate.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception {ex.Message} occured for Get Request for delivery state {request.State} and order number {request.Order.OrderNumber}");
                throw;
            }

          

        }
    }
}
