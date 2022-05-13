using System;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Validations;

namespace PaymentGateway.Domain.UnitTest
{
    [TestClass]
    public class CardValidatorTest
    {
        private CardValidation _validator = null;
        public CardValidatorTest() => _validator = new CardValidation();

        //Arrange
        private readonly CardDetails cardModel = new CardDetails()
        {
            CVV = "123",
            Number = "1234567812345678",
            BeneficiaryName = "Dheeraj Bansal",
            ExpiryDate = new ExpiryDate(02, 2022),
        };

        [TestMethod]
        public void Is_Invalid_CVV()
        {
            //Arrange
            cardModel.CVV = "12345";

            //Act
            ValidationResult result = _validator.Validate(cardModel);

            //Assert
            Assert.IsNotNull(result.Errors);
        }

        [TestMethod]
        public void Should_Return_Success_For_Valid_CVV()
        {
            //Act
            ValidationResult result = _validator.Validate(cardModel);

            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Is_Invalid_CardNumber()
        {
            //Arrange
            cardModel.Number = "12345454";

            //Act
            ValidationResult result = _validator.Validate(cardModel);

            //Assert
            Assert.IsNotNull(result.Errors);
        }

        [TestMethod]
        public void Should_Return_Success_For_Valid_CardNumber()
        {
            //Act
            ValidationResult result = _validator.Validate(cardModel);

            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Should_Return_Error_For_Invalid_BeneficiaryName()
        {
            //Arrange
            cardModel.BeneficiaryName = string.Empty;

            //Act
            ValidationResult result = _validator.Validate(cardModel);

            //Assert
            Assert.IsNotNull(result.Errors);
        }
    }
}
