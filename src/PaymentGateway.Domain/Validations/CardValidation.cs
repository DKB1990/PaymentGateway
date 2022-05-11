using System;
using FluentValidation;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Domain.Validations
{
    public class CardValidation : AbstractValidator<CardDetails>
    { 
        public CardValidation()
        {
            RuleFor(x => x.CVV).Length(3, 4).NotEmpty().WithMessage("Invalid CVV");
            RuleFor(x => x.BeneficiaryName).NotEmpty().WithMessage("Invalid CVV");
            RuleFor(x => x.Number).Length(16).WithMessage("Invalid Card Number");
        }
    }
}