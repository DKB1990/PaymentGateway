using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Services;

namespace PaymentGateway.Domain.UnitTest
{
    [TestClass]
    public class CardMaskServiceTest
    {
        [TestMethod]
        public void Should_Return_MaskedCardNumber()
        {
            //Arrange & Act
            string cardNumber = "5555444433332222";
            string maskedCardNumber = CardMaskService.CardMask(cardNumber);

            //Assert
            Assert.IsTrue(maskedCardNumber.Length == 16);
            Assert.AreSame("********", maskedCardNumber.Substring(4, 8));
        }
    }
}
