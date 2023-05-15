using AutoMapper;
using FluentAssertions;
using Moq;
using TT.Deliveries.Application.Commands.CreateDelivery;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Exceptions;
using TT.Deliveries.Application.Logging;
using TT.Deliveries.Application.MappingProfiles;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.UnitTests.Commands
{
    public class CreateDeliveryCommandHandlerTests
    {
        private readonly Mock<IDeliveryRepository> _repository;
        private readonly Mock<IAppLogger<CreateDeliveryCommandHandler>> _logger;
        private readonly IMapper _mapper;

        public CreateDeliveryCommandHandlerTests()
        {
            _repository = new Mock<IDeliveryRepository>();
            _logger = new Mock<IAppLogger<CreateDeliveryCommandHandler>>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DeliveryProfile());
            });

            _mapper = mockMapper.CreateMapper();

            _repository.Setup(r => r.CreateAsync(It.IsAny<Delivery>()));
            _logger.Setup(r => r.LogInformation(It.IsAny<string>()));
            _logger.Setup(l => l.LogError(It.IsAny<string>()));

        }

        [Fact]
        public async Task Successfully_Create_Delivery_When_Valid_Request_Receives()
        {
            var createDeliveryCommandHandler = new CreateDeliveryCommandHandler(_mapper, _repository.Object, _logger.Object);

            await createDeliveryCommandHandler.Handle(GetCreateDeliveryCommand(), CancellationToken.None);

            _repository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Once);
            _logger.Verify(r => r.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Throw_BadRequest_Exception_When_Invalid_DeliveryState_Receives()
        {
            var createDeliveryCommandHandler = new CreateDeliveryCommandHandler(_mapper, _repository.Object, _logger.Object);

            var deliveryCommand = GetCreateDeliveryCommand();

            deliveryCommand.State = "rejected";

            Func<Task> act = async () => await createDeliveryCommandHandler.Handle(deliveryCommand, CancellationToken.None);

            await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid Delivery Creation Request");
            _logger.Verify(r => r.LogError(It.IsAny<string>()), Times.AtLeastOnce);
            _repository.Verify(r => r.CreateAsync(It.IsAny<Delivery>()), Times.Never);
            
        }

        private static CreateDeliveryCommand GetCreateDeliveryCommand()
        {
            return new CreateDeliveryCommand
            {
                State = "created",
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
