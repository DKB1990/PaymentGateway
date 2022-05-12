using System;
using System.Threading.Tasks;
using AutoMapper;
using PaymentGateway.API.CQRS.Events;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.SimulatorBank;
using PaymentGateway.SimulatorBank.Models;

namespace PaymentGateway.API.CQRS.Handlers
{
    public class MakePaymentEventDispatcherHandler : IPaymentEventDispatcher
    {
        private readonly IMapper _mapper;
        private readonly IBankConnector _connector;
        private readonly IPaymentRepository _paymentRepository;

        public MakePaymentEventDispatcherHandler(IMapper mapper, IPaymentRepository paymentRepository, IBankConnector connector)
        {
            _mapper = mapper;
            _connector = connector;
            _paymentRepository = paymentRepository;
        }

        public Task DispatchAsync<IEvent>(IEvent Event)
        {
            if (Event.GetType() == typeof(MakePaymentEvent))
            {
                Task task = Task.Run(() => ProcessPayment((Event as MakePaymentEvent).Payment));
            }
            else
                throw new NotSupportedException("Unsupported Event");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Process the payment by sending it to the bank
        /// </summary>
        /// <param name="payment">The payment being process</param>
        /// <returns></returns>
        public async Task ProcessPayment(Payment payment)
        {
            try
            {
                var bankRequest = _mapper.Map<BankPaymentRequest>(payment);
                BankPaymentResponse response = await _connector.ProcessPaymentAsync(bankRequest);

                if (response.StatusCode == PaymentStatusCode.Approved)
                    payment.StatusCode = PaymentStatusCode.Approved;
                else
                {
                    payment.StatusCode = PaymentStatusCode.Declined;
                    payment.DeclinedReasonCode = response.DeclinedReasonCode;
                }

                await _paymentRepository.UpdateAsync(payment);
            }
            catch (Exception ex)
            {
                throw new Exception("Mock Acquiring Bank Unavailable");
            }
        }
    }
}
