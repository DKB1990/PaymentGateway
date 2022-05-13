using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PaymentGateway.API.Commands;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Queries;
using PaymentGateway.API.Responses;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.API.IntegrationTest
{
    [TestClass]
    public class GetPaymentTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ApiFixture _fixture;
        private readonly Mock<IPaymentRepository> _paymentRepository;

        public GetPaymentTest()
        {
            _fixture = new ApiFixture();
            _mediator = new Mock<IMediator>();
            _paymentRepository = new Mock<IPaymentRepository>();
        }

        [TestMethod]
        public async Task Should_Return_Valid_PaymentResponse()
        {
            //Arrange
            Guid paymentId = Guid.NewGuid();
            Guid merchantId = Guid.NewGuid();
            MakePaymentCommand paymentCommand = new MakePaymentCommand()
            {
                Id = paymentId,
                Amount = 500.5M,
                CurrencyCode = "GBP",
                Description = "Test GetPayment",
                CardDetails = new MakePaymentCommand.CardDetail()
                {
                    CVV = "887",
                    Number = "4444333344445555",
                    BeneficiaryName = "Dheeraj Bansal",
                    ExpiryDate = new MakePaymentCommand.CardExpiryDate()
                    {
                        Month = 10,
                        Year = 2024
                    }
                }
            };

            var client = _fixture.factory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");
            await client.PostAsync("/Payments", new StringContent(JsonConvert.SerializeObject(paymentCommand), Encoding.UTF8, "application/json"));

            // Act
            var response = await client.GetAsync("/Payments/" + paymentId.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<GetPaymentResponse>(json);

            string maskedCardNumber = "4444********5555";

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(paymentResponse.Id, paymentId);
            Assert.AreEqual(paymentResponse.Amount, 500.5M);
            Assert.AreEqual(paymentResponse.CurrencyCode, "GBP");
            Assert.AreEqual(paymentResponse.CardNumber, maskedCardNumber);
            Assert.AreEqual(paymentResponse.BeneficiaryName, "Dheeraj Bansal");

            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task Should_Return_Invalid_PaymentId()
        {
            //Arrange
            GetPaymentQuery query = new GetPaymentQuery();
            var controller = new PaymentReadController(_mediator.Object);

            //Act
            var actionResult = await controller.GetPayment(query);

            //Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundObjectResult));
            await Task.CompletedTask;
        }
    }
}
