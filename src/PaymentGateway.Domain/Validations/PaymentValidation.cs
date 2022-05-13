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
            RuleFor(x => x.Amount).GreaterThan(1M).WithMessage("Invalid Amount");
            RuleFor(x => x.Description).MinimumLength(5).WithMessage("Invalid Description");
            RuleFor(x => x.CurrencyCode).Length(3).WithMessage("Invalid Currency Code").Must(isValidCurrencyCode).WithMessage("Unsupported Currency Code");
        }

        private bool isValidCurrencyCode(string currencyCode)
        {
            bool result = allowedCurrencies.Contains(currencyCode);
            return result;
        }
    }
}
