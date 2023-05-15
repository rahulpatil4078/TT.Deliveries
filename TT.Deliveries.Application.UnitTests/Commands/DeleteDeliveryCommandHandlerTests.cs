using Moq;
using TT.Deliveries.Application.Commands.DeleteDelivery;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Logging;

namespace TT.Deliveries.Application.UnitTests.Commands
{
    public class DeleteDeliveryCommandHandlerTests
    {
        private readonly Mock<IDeliveryRepository> _repository;
        private readonly Mock<IAppLogger<DeleteDeliveryCommandHandler>> _logger;
      
        public DeleteDeliveryCommandHandlerTests()
        {
            _repository = new Mock<IDeliveryRepository>();
            _logger = new Mock<IAppLogger<DeleteDeliveryCommandHandler>>();

            _repository.Setup(r => r.DeleteAsync(It.IsAny<string>()));
            _logger.Setup(r => r.LogInformation(It.IsAny<string>()));
            _logger.Setup(l => l.LogError(It.IsAny<string>()));

        }

        [Fact]
        public async Task Successfully_Delete_Delivery_When_Valid_Request_Receives()
        {
            var createDeliveryCommandHandler = new DeleteDeliveryCommandHandler(_repository.Object, _logger.Object);

            await createDeliveryCommandHandler.Handle(new DeleteDeliveryCommand { OrderNumber = "1234567"}, CancellationToken.None);

            _repository.Verify(r => r.DeleteAsync(It.IsAny<string>()), Times.Once);
            _logger.Verify(r => r.LogInformation(It.IsAny<string>()), Times.Once);
        }

       
    }
}
