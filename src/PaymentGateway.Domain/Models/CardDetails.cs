using System;
namespace PaymentGateway.Domain.Models
{
    public class CardDetails
    {
        public string Number { get; set; }
        public string CVV { get; set; }
        public ExpiryDate ExpiryDate { get; set; }
        public string BeneficiaryName { get; set; }
    } 
}
