using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PaymentGateway.API.IntegrationTest
{
    [TestClass]
    public class GetPaymentTest
    {
        public GetPaymentTest() { }

        [TestMethod]
        public async Task Should_Return_Valid_PaymentResponse()
        {
            await Task.CompletedTask;
        }

        [TestMethod]
        public async Task Should_Return_Invalid_PaymentId()
        {
            await Task.CompletedTask;
        }
    }
}
