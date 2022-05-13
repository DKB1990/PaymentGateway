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
        public async Task Make_Payment_Returns_Success_When_ValidData()
        {
            // Arrange & Act
            var response = await executeAndGetResults(paymentCommand);
            var json = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<MakePaymentResponse>(json);

            // Assert
            Assert.IsNotNull(paymentResponse.Id);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(paymentResponse.StatusCode, "Payment processing");
        }

        [TestMethod]
        public async Task Make_Payment_Returns_Error_When_InvalidCurrencyCode()
        {
            // Arrange
            paymentCommand.CurrencyCode = "ASD"; //Unsupported Currency Code.

            //Act
            var response = await executeAndGetResults(paymentCommand);
            var errorMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage.Replace("\"", ""), "Payment failed:: Unsupported Currency Code");
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_Currency_InValid()
        {
            // Arrange
            paymentCommand.CurrencyCode = "ASSD"; //Unsupported Currency Code.
            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errorMessage = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage.Replace("\"", ""), "Payment failed:: Invalid Currency Code");
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_Amount_InValid()
        {
            // Arrange
            paymentCommand.Amount = 0; //Unsupported Currency Code.

            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errorMessage = await response.Content.ReadAsStringAsync();
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage.Replace("\"", ""), "Payment failed:: Invalid Amount");
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_CardNumber_InValid()
        {
            // Arrange
            paymentCommand.CardDetails.Number = "44445555666";

            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errorMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage.Replace("\"", ""), "Payment failed:: Invalid Card Number");
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_CVV_InValid()
        {
            // Arrange
            paymentCommand.CardDetails.CVV = "99999";

            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errorMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage.Replace("\"", ""), "Payment failed:: 'CVV' must be between 3 and 4 characters. You entered 5 characters.");
        }

        [TestMethod]
        public async Task Make_Payment_Returns_BadRequest_When_ExpiryDate_InValid()
        {
            // Arrange
            paymentCommand.CardDetails.ExpiryDate = new MakePaymentCommand.CardExpiryDate()
            {
                Year = 2021,
                Month = 10
            };

            // Act
            var response = await executeAndGetResults(paymentCommand);
            var errorMessage = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.BadRequest);
            Assert.AreEqual(errorMessage.Replace("\"", ""), "Payment failed:: Invalid Expiry Date");
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
