using System;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.API.CQRS.Events;
using PaymentGateway.API.CQRS.Handlers;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.SimulatorBank;
using PaymentGateway.SimulatorBank.Models;

namespace PaymentGateway.API.UnitTest
{
    [TestClass]
    public class MakePaymentEventDispatcherTest
    {
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IPaymentRepository> _repositoryMock = new Mock<IPaymentRepository>();
        private readonly Mock<IBankConnector> _adaptarMock = new Mock<IBankConnector>();
        private readonly IPaymentEventDispatcher _dispatcher;

        public MakePaymentEventDispatcherTest()
        {
            _dispatcher = new MakePaymentEventDispatcherHandler( _mapperMock.Object, _repositoryMock.Object);
        }

        [TestMethod]
        public void Publish_Should_Dispatch_Event_If_Valid()
        {
            // Arrange
            var payment = new Payment()
            {
                Id = Guid.NewGuid(),
                Amount = 500.50M,
                CurrencyCode = "AED",
                Description = "Apple Tokenized Payment",
                CardDetails = new CardDetails()
                {
                    Number = "3792120488774391",
                    CVV = "998",
                    ExpiryDate = new ExpiryDate(2022, 02),
                    BeneficiaryName = "Dheeraj Bansal"
                }
            };

            var bankRequest = new BankPaymentRequest
            {
                Amount = payment.Amount,
                CurrencyCode = payment.CurrencyCode,
                PaymentId = payment.Id.ToString(),
                Description = payment.Description,
                MerchantId = Guid.NewGuid().ToString(),
                CardDetails = new CardDetail()
                {
                    CVV = payment.CardDetails.CVV,
                    Number = payment.CardDetails.Number,
                    BeneficiaryName = payment.CardDetails.BeneficiaryName,
                    ExpiryDate = new CardExpiryDate(payment.CardDetails.ExpiryDate.Year, payment.CardDetails.ExpiryDate.Month),
                },
            };

            var bankResponse = new BankPaymentResponse()
            {
                PaymentId = bankRequest.PaymentId,
                StatusCode = PaymentStatusCode.Approved
            };

            _adaptarMock.Setup(x => x.ProcessPaymentAsync(bankRequest)).ReturnsAsync(bankResponse);

            // Act
            Action action = () => _dispatcher.DispatchAsync(new MakePaymentEvent(payment));

            // Assert
            action.Invoke();
        }
    }
}
