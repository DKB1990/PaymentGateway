using System;
using MediatR;
using PaymentGateway.API.Responses;

namespace PaymentGateway.API.Queries
{
    public class GetPaymentQuery : IRequest<GetPaymentResponse>
    {
        public Guid Id { get; set; }
        internal Guid MerchantId { get; set; }
    }
}
