using System.Threading.Tasks;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentEventDispatcher
    {
        public Task DispatchAsync<IEvent>(IEvent Event);
    }

    public interface IEvent
    {
    }
}
