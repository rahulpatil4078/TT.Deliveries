using MediatR;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Logging;

namespace TT.Deliveries.Application.Commands.DeleteDelivery
{
    public class DeleteDeliveryCommandHandler : IRequestHandler<DeleteDeliveryCommand, Unit>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IAppLogger<DeleteDeliveryCommandHandler> _logger;
        public DeleteDeliveryCommandHandler(IDeliveryRepository deliveryRepository, IAppLogger<DeleteDeliveryCommandHandler> logger)
        {
            _deliveryRepository = deliveryRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteDeliveryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _deliveryRepository.DeleteAsync(request.OrderNumber);

                _logger.LogInformation($"Delivery deleted successfully for OrderNumber {request.OrderNumber}");

                return Unit.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception {ex.Message} occured for delete Request for order number {request.OrderNumber}");
                throw;
            }
  

        }
    }
}
