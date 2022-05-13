using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Queries;
using PaymentGateway.API.Responses;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("Payments")]
    public class PaymentReadController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentReadController(IMediator mediator) => _mediator = mediator;

        /// <summary>
        /// Request details of a previous payments based on Id.
        /// </summary>
        /// <param name="query">PaymentId to get the payment request.</param>
        /// <returns>A payment</returns>
        [HttpGet("{Id}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(GetPaymentResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPayment([FromRoute] GetPaymentQuery query)
        {
            query.MerchantId = Guid.NewGuid();//User?.FindFirstValue("MerchantId").ToString();
            var result = await _mediator.Send(query);

            if (result is null)
                return NotFound($"Payment with id {query.Id} not found");

            return Ok(result);
        }

    }
}
