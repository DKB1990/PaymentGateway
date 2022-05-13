using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Validations;

namespace PaymentGateway.Domain.UnitTest
{
    [TestClass]
    public class ExpiryDateValidator
    {
        private ExpiryDateValidation _validator = null;
        public ExpiryDateValidator() => _validator = new ExpiryDateValidation();

        [TestMethod]
        public void Should_Return_Error_For_Invalid_ExpiryDate()
        {
            //Arrange
            ExpiryDate date = new ExpiryDate(10, 2021);
            //Act
            ValidationResult result = _validator.Validate(date);
            //Assert
            Assert.IsNotNull(result.Errors);
        }

        [TestMethod]
        public void Should_Return_Success_For_Valid_ExpiryDate()
        {
            //Arrange
            ExpiryDate date = new ExpiryDate(01, 2023);

            //Act
            ValidationResult result = _validator.Validate(date);

            //Assert
            Assert.IsTrue(result.IsValid);
        }
    }
}
