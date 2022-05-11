using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.API.CQRS.Events
{
    public class MakePaymentEvent : IEvent
    {
        public Payment Payment { get; private set; }
        public MakePaymentEvent(Payment payment) => Payment = payment;
    }
}
