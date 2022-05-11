using System;
using System.Threading.Tasks;
using PaymentGateway.SimulatorBank.Models;

namespace PaymentGateway.SimulatorBank
{
    public interface IBankConnector
    {
        public Task<BankPaymentResponse> ProcessPaymentAsync(BankPaymentRequest request);
    }
}
