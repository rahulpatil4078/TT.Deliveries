using AutoMapper;
using FluentAssertions;
using Moq;
using TT.Deliveries.Application.Contracts;
using TT.Deliveries.Application.Exceptions;
using TT.Deliveries.Application.Logging;
using TT.Deliveries.Application.MappingProfiles;
using TT.Deliveries.Application.Queries;
using TT.Deliveries.Domain;

namespace TT.Deliveries.Application.UnitTests.Queries
{
    public class GetDeliveryDetailsQueryHandlerTests
    {
        private readonly Mock<IDeliveryRepository> _repository;
        private readonly Mock<IAppLogger<GetDeliveryDetailsQueryHandler>> _logger;
        private readonly IMapper _mapper;

        public GetDeliveryDetailsQueryHandlerTests()
        {
            _repository = new Mock<IDeliveryRepository>();
            _logger = new Mock<IAppLogger<GetDeliveryDetailsQueryHandler>>();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DeliveryProfile());
            });

            _mapper = mockMapper.CreateMapper();

            _repository.Setup(r => r.GetByStateAsync(It.IsAny<DeliveryState>())).
                ReturnsAsync(new List<Delivery>
                {
                    new Delivery
                    {
                  State = DeliveryState.Created,
                  AccessWindow = new AccessWindow(),
                  Order= new Order{OrderNumber = "1234577" },
                  Recipient = new Recipient(),
                    },
                     new Delivery
                    {
                  State = DeliveryState.Created,
                  AccessWindow = new AccessWindow(),
                  Order= new Order {OrderNumber = "12345678"},
                  Recipient = new Recipient(),
                    }
                });

            _logger.Setup(r => r.LogInformation(It.IsAny<string>()));
            _logger.Setup(l => l.LogError(It.IsAny<string>()));

        }

        [Fact]
        public async Task Successfully_Get_Delivery_When_Valid_Request_Receives()
        {
            var GetDeliveryCommandHandler = new GetDeliveryDetailsQueryHandler(_mapper, _repository.Object, _logger.Object);

           var result = await GetDeliveryCommandHandler.Handle(new GetDeliveryDetailsQuery("created"), CancellationToken.None);


            result.Should().NotBeNull();
            result.Count.Should().Be(2);

            _repository.Verify(r => r.GetByStateAsync(It.IsAny<DeliveryState>()), Times.Once);
            _logger.Verify(r => r.LogInformation(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Throw_BadRequest_Exception_When_Invalid_DeliveryState_Receives()
        {
            var GetDeliveryCommandHandler = new GetDeliveryDetailsQueryHandler(_mapper, _repository.Object, _logger.Object);

            Func<Task> act = async () => await GetDeliveryCommandHandler.Handle(new GetDeliveryDetailsQuery("rejected"), CancellationToken.None);

            await act.Should().ThrowAsync<BadRequestException>().WithMessage("Invalid Delivery State received in request");
            _logger.Verify(r => r.LogError(It.IsAny<string>()), Times.AtLeastOnce);
            _repository.Verify(r => r.GetByStateAsync(It.IsAny<DeliveryState>()), Times.Never);

        }

       
    }
}
