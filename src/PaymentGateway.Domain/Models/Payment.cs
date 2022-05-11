using System;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Domain.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid MerchatId { get; set; }
        public CardDetails CardDetails { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public PaymentStatusCode StatusCode { get; set; }
        public string Description { get; set; }
        public PaymentDeclinedReasonCode DeclinedReasonCode { get; set; }
        public DateTime RequestedDateTime => DateTime.UtcNow;
    }
}
