using System;
using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Validations
{
    public class ExpiryDateValidation : AbstractValidator<ExpiryDate>
    {
        public ExpiryDateValidation()
        {
            RuleFor(x => x.Year).Must((expirydate, year) => isExpiryDateMonthValid(expirydate.Month, year)).WithMessage("Invalid Expiry Date");
        }

        private bool isExpiryDateMonthValid(int month, int year)
        {
            DateTime utcDateTime = DateTime.UtcNow;
            if (month < 1 || month > 12 || year < utcDateTime.Year)
                return false;

            if (year == utcDateTime.Year && month >= utcDateTime.Month)
                return false;

            return true;
        }
    }
}
