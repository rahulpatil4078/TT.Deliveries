using MediatR;
using Microsoft.AspNetCore.Mvc;
using TT.Deliveries.Application.Commands.CreateDelivery;
using TT.Deliveries.Application.Commands.DeleteDelivery;
using TT.Deliveries.Application.Commands.UpdateDelivery;
using TT.Deliveries.Application.Queries;

namespace TT.Deliveries.Web.Api.Controllers
{
    [Route("deliveries")]
    [ApiController]
    [Produces("application/json")]
    public class DeliveriesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public DeliveriesController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<DeliveryDetailsDto>>> Get(string state)
        {
            var deliveries = await _mediator.Send(new GetDeliveryDetailsQuery(state));
            return Ok(deliveries);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(CreateDeliveryCommand createDelivery)
        {
            var response = await _mediator.Send(createDelivery);
            return CreatedAtAction(nameof(Post), new { id = response });
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Put(UpdateDeliveryCommand updateDelivery)
        {
            await _mediator.Send(updateDelivery);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(string orderNumber)
        {
            var command = new DeleteDeliveryCommand { OrderNumber = orderNumber };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}