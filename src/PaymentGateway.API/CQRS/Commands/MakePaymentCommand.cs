using System;
using MediatR;
using PaymentGateway.API.Responses;

namespace PaymentGateway.API.Commands
{
    public class MakePaymentCommand : IRequest<MakePaymentResponse>
    {
        public Guid Id { get; set; }
        internal Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }
        public CardDetail CardDetails { get; set; }

        public class CardDetail
        {
            public string Number { get; set; }
            public string CVV { get; set; }
            public CardExpiryDate ExpiryDate { get; set; }
            public string BeneficiaryName { get; set; }
        }

        public class CardExpiryDate
        {
            public int Year { get; set; }
            public int Month { get; set; }
        }
    }
}
