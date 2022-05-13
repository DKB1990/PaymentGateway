using System;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Validations;

namespace PaymentGateway.Domain.UnitTest
{
    [TestClass]
    public class PaymentModelTest
    {
        private PaymentValidation _validator = null;
        public PaymentModelTest() => _validator = new PaymentValidation();

        [TestMethod]
        public void Should_Return_Success_If_Valid_Payment()
        {
            //Arrange
            Payment payModel = new Payment()
            {
                Amount = 10,
                CurrencyCode = "GBP",
                Description = "Test Description",
                Id = Guid.NewGuid(),
                MerchatId = Guid.NewGuid(),
            };

            //Act
            ValidationResult result = _validator.Validate(payModel);
            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_Return_Error_If_Valid_Payment()
        {
            //Arrange
            Payment payModel = new Payment()
            {
                Amount = 0,
                CurrencyCode = "GPBP",
                Description = "T",
                Id = Guid.NewGuid(),
                MerchatId = Guid.NewGuid(),
            };

            //Act
            ValidationResult result = _validator.Validate(payModel);
            //Assert
            Assert.IsNotNull(result.Errors);
        }
    }
}
