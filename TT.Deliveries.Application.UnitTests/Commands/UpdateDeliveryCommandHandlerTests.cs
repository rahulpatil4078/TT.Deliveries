using AutoMapper;
using FluentAssertions;
using Moq;
using TT.Deliveries.Application.Commands.CreateDelivery;
using TT.Deliveries.Application.Commands.UpdateDelivery;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Exceptions;
using TT.Deliveries.Application.Logging;
using TT.Deliveries.Application.MappingProfiles;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.UnitTests.Commands
{
    public class UpdateDeliveryCommandHandlerTests
    {
        private readonly Mock<IDeliveryRepository> _repository;
        private readonly Mock<IAppLogger<UpdateDeliveryCommandHandler>> _logger;
        private readonly IMapper _mapper;

        public UpdateDeliveryCommandHandlerTests()
        {
            _repository = new Mock<IDeliveryRepository>();
            _logger = new Mock<IAppLogger<UpdateDeliveryCommandHandler>>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DeliveryProfile());
            });

            _mapper = mockMapper.CreateMapper();

            _repository.Setup(r => r.UpdateAsync(It.IsAny<Delivery>(), It.IsAny<List<DeliveryState>>()));
            _logger.Setup(r => r.LogInformation(It.IsAny<string>()));
            _logger.Setup(l => l.LogError(It.IsAny<string>()));

        }

        [Fact]
        public async Task Successfully_Update_Delivery_When_Valid_Request_Receives()
        {
            var createDeliveryCommandHandler = new UpdateDeliveryCommandHandler(_mapper, _repository.Object, _logger.Object);

            await createDeliveryCommandHandler.Handle(GetUpdateDeliveryCommand(), CancellationToken.None);

            _repository.Verify(r => r.UpdateAsync(It.IsAny<Delivery>(), It.IsAny<List<DeliveryState>>()), Times.Once);
            _logger.Verify(r => r.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Throw_BadRequest_Exception_When_Invalid_DeliveryState_Receives()
        {
            var updateDeliveryCommandHandler = new UpdateDeliveryCommandHandler(_mapper, _repository.Object, _logger.Object);

            var deliveryCommand = GetUpdateDeliveryCommand();

            deliveryCommand.State = "rejected";

            Func<Task> act = async () => await updateDeliveryCommandHandler.Handle(deliveryCommand, CancellationToken.None);

            await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid Delivery Update Request");
            _logger.Verify(r => r.LogError(It.IsAny<string>()), Times.AtLeastOnce);
            _repository.Verify(r => r.UpdateAsync(It.IsAny<Delivery>(), It.IsAny<List<DeliveryState>>()), Times.Never);
            
        }

        private static UpdateDeliveryCommand GetUpdateDeliveryCommand()
        {
            return new UpdateDeliveryCommand
            {
                State = "approved",
                AccessWindow = new Shared.AccessWindow 
                { 
                    EndTime = DateTime.UtcNow, 
                    StartTime = DateTime.UtcNow 
                },
                Recipient = new Shared.Recipient 
                { 
                    Address = "Flat 201, Wembley, London",
                    Email = "abc@gmail.com", 
                    Name = "John", 
                    PhoneNumber = "+44567890213" 
                },
                Order = new Shared.Order
                {
                    OrderNumber = "12345678",
                    Sender = "Ikea"
                }

            };
        }
    }
}
