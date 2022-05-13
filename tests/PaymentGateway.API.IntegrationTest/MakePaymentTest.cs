using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using PaymentGateway.API.Commands;
using PaymentGateway.API.Responses;

namespace PaymentGateway.API.IntegrationTest
{
    [TestClass]
    public class MakePaymentTest
    {
        private readonly ApiFixture _fixture;
        private readonly MakePaymentCommand paymentCommand = new MakePaymentCommand()
        {
            Id = Guid.NewGuid(),
            Amount = 50,
            CurrencyCode = "AED",
            Description = "Test Initiated Payment",
            CardDetails = new MakePaymentCommand.CardDetail()
            {
                CVV = "887",
                Number = "4444333322224444",
                BeneficiaryName = "Dheeraj Bansal",
                ExpiryDate = new MakePaymentCommand.CardExpiryDate()
                {
                    Year = 2024,
                    Month = 10,
                },
            }
        };

        public MakePaymentTest() => _fixture = new ApiFixture();

        [TestMethod]
        public async Task Make_Payment_Returns_Correct_Data_When_Valid()
        {
            // Arrange & Act
            var response = await executeAndGetResults(paymentCommand);
            var json = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<MakePaymentResponse>(json);

            // Assert
            Assert.IsNotNull(paymentResponse.Id);
            Assert.Equals(response.StatusCode, HttpStatusCode.Created);
            Assert.Equals(paymentResponse.StatusCode, "Payment processing");
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_Currency_InValid()
        {
            // Arrange
            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errormessage = await response.Content.ReadAsStringAsync();
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_Amount_InValid()
        {
            // Arrange
            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errormessage = await response.Content.ReadAsStringAsync();
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_CardNumber_InValid()
        {
            // Arrange
            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equals(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_CVV_InValid()
        {
            // Arrange
            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equals(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_ExpiryDate_InValid()
        {
            // Arrange
            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equals(response.StatusCode, HttpStatusCode.BadRequest);
        }


        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_ExpiryDate_Expired()
        {
            // Arrange
            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equals(response.StatusCode, HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_Duplicate_Payment()
        {
            // Arrange

            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equals(response.StatusCode, HttpStatusCode.BadRequest);
        }

        private async Task<HttpResponseMessage> executeAndGetResults(MakePaymentCommand command)
        {
            var client = _fixture.factory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            //Act
            var payload = JsonConvert.SerializeObject(command);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            return response;
        }
    }
}
