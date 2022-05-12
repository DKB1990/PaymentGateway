using System;

namespace PaymentGateway.API.Responses
{
    public class GetPaymentResponse
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string StatusCode { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public string DeclinedReasonCode { get; set; }
        public string CardExpiryDate { get; set; }
        public string CardNumber { get; set; }
        public string BeneficiaryName { get; set; }
        public DateTime RequestedDateTime { get; set; }
        public bool IsPaymentDeclined => !string.IsNullOrWhiteSpace(DeclinedReasonCode);
    }
}
