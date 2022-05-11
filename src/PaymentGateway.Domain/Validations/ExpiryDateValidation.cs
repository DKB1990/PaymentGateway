using System;
using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Validations
{
    public class ExpiryDateValidation : AbstractValidator<ExpiryDate>
    {
        public ExpiryDateValidation()
        {
            RuleFor(x => x.Year).LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Invalid Expiry Date: YEAR");
            RuleFor(x => x.Year).Must((expirydate, year) => isExpiryDateMonthValid(expirydate.Month, year)).WithMessage("Invalid Expiry Date: MONTH");
        }

        private bool isExpiryDateMonthValid(int month, int year)
        {
            if (month < 1 || month > 12)
                return false;

            DateTime utcDateTime = DateTime.UtcNow;
            if (year == utcDateTime.Year && month <= utcDateTime.Month)
                return false;

            return true;
        }
    }
}
