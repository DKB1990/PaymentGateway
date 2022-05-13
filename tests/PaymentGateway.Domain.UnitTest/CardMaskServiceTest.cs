using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Queries;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;
using PaymentGateway.Infrastructure.Repositories;

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
            Assert.AreEqual("********", maskedCardNumber.Substring(4, 8));
        }
    }
}
