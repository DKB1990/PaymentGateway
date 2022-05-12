using PaymentGateway.Domain.Enums;

namespace PaymentGateway.SimulatorBank.Models
{
    public class BankPaymentResponse
    {
        public string PaymentId { get; set; }
        public string MerchantId { get; set; }
        public PaymentStatusCode StatusCode { get; set; }
        public PaymentDeclinedReasonCode? DeclinedReasonCode { get; set; }
    }
}
