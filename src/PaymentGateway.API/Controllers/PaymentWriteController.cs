using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Commands;
using PaymentGateway.API.Responses;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("Payments")]
    public class PaymentWriteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentWriteController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Make a payment to a merchant
        /// </summary>
        /// <param name="command">the payment being sent</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(201, Type = typeof(MakePaymentResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentCommand command)
        {
            command.MerchantId = Guid.NewGuid();// ?? Guid.Parse(User.FindFirstValue("MerchantId"));
            var result = await _mediator.Send(command);

            //TODO: refactor this into a bool and map
            if (result.StatusCode.Contains("failed"))
                return BadRequest(result.StatusCode);
            else
                return CreatedAtAction("MakePayment", new { paymentId = result.Id }, result);
        }
    }
}
