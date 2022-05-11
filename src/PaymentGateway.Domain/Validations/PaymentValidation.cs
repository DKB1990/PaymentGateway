using System.Collections.Generic;
using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Validations
{
    public class PaymentValidation : AbstractValidator<Payment>
    {
        private static readonly HashSet<string> allowedCurrencies = new HashSet<string>()
        {
            "GBP", "AED", "INR", "USD", "EUR"
        };

        public PaymentValidation()
        {
            RuleFor(x => x.Amount).LessThan(1).WithMessage("Invalid Amount");
            RuleFor(x => x.Description).MinimumLength(5).WithMessage("Invalid Description");
            RuleFor(x => x.CurrencyCode).Length(3).Must(isValidCurrencyCode).WithMessage("Invalid Currency Code");
        }

        private bool isValidCurrencyCode(string currencyCode)
        {
            return allowedCurrencies.Contains(currencyCode);
        }
    }
}
