using System;

namespace PaymentGateway.API.Responses
{
    public class MakePaymentResponse
    {
        public Guid Id { get; set; }
        public string StatusCode { get; set; }
    }
}