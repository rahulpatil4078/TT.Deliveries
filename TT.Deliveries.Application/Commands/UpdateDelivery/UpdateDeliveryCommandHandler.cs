using AutoMapper;
using MediatR;
using TT.Deliveries.Application.Commands.UpdateDelivery;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Exceptions;
using TT.Deliveries.Application.Logging;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.Commands.CreateDelivery
{
    public class UpdateDeliveryCommandHandler : IRequestHandler<UpdateDeliveryCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IAppLogger<UpdateDeliveryCommandHandler> _logger;
        public UpdateDeliveryCommandHandler(IMapper mapper, IDeliveryRepository deliveryRepository,
            IAppLogger<UpdateDeliveryCommandHandler> logger) 
        {
            _mapper = mapper;
            _deliveryRepository = deliveryRepository;
            _logger = logger;
        }

       public async Task<Unit> Handle(UpdateDeliveryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateDeliveryCommandValidator();
                var validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (validationResult.Errors.Any())
                {
                    _logger.LogError($"Invalid Delivery Update Request for order number {request.Order.OrderNumber}");
                    throw new BadRequestException("Invalid Delivery Update Request", validationResult);
                }

                var deliveryToUpdate = _mapper.Map<Delivery>(request);

                var deliveryStatefilters = BuildDeliveryStateFilters(deliveryToUpdate);

                await _deliveryRepository.UpdateAsync(deliveryToUpdate, deliveryStatefilters);

                _logger.LogInformation($"Delivery updated successfully with state as {request.State} for OrderNumber {request.Order.OrderNumber}");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception {ex.Message} occured for Get Request for delivery state {request.State} and order number {request.Order.OrderNumber}");
                throw;
            }

          

        }

        private static List<DeliveryState> BuildDeliveryStateFilters(Delivery deliveryToUpdate)
        {
            var lstDeliveryStatefilters = new List<DeliveryState>();

            switch (deliveryToUpdate.State)
            {
                case DeliveryState.Approved:
                    lstDeliveryStatefilters.Add(DeliveryState.Created);
                    break;

                case DeliveryState.Completed:
                    lstDeliveryStatefilters.Add(DeliveryState.Approved);
                    break;

                case DeliveryState.Cancelled:
                    lstDeliveryStatefilters.AddRange(new[] { DeliveryState.Created, DeliveryState.Approved });
                    break;
            }

            return lstDeliveryStatefilters;
        }
    }
}
