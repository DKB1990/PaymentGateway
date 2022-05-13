using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Commands;
using PaymentGateway.API.CQRS.Events;
using PaymentGateway.API.Responses;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;
using PaymentGateway.Domain.Validations;

namespace PaymentGateway.API.CQRS.Commands
{
    public class MakePaymentCommandHandler : IRequestHandler<MakePaymentCommand, MakePaymentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentEventDispatcher _dispatcher;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<MakePaymentCommandHandler> _logger;
        public MakePaymentCommandHandler(IMapper mapper,
                IPaymentRepository paymentRepository,
                ILogger<MakePaymentCommandHandler> logger, IPaymentEventDispatcher dispatcher)
        {
            _mapper = mapper;
            _logger = logger;
            _dispatcher = dispatcher;
            _paymentRepository = paymentRepository;
        }

        public async Task<MakePaymentResponse> Handle(MakePaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentResponse = new MakePaymentResponse();

            try
            {
                Payment payment = new Payment()
                {
                    Id = request.Id,
                    MerchatId = request.MerchantId,
                    Amount = request.Amount,
                    CurrencyCode = request.CurrencyCode,
                    Description = request.Description,
                    CardDetails = new Domain.Models.CardDetails()
                    {
                        CVV = request.CardDetails.CVV,
                        Number = request.CardDetails.Number,
                        ExpiryDate = new Domain.Models.ExpiryDate(request.CardDetails.ExpiryDate.Month, request.CardDetails.ExpiryDate.Year),
                        BeneficiaryName = request.CardDetails.BeneficiaryName,
                    }
                };

                //Validating the Payment Model.
                PaymentValidation paymentValidation = new PaymentValidation();
                ValidationResult paymentValidationResult = paymentValidation.Validate(payment);
                if (!paymentValidationResult.IsValid)
                    throw new HttpRequestException(paymentValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage,
                        new Exception(paymentValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage),
                        HttpStatusCode.BadRequest);

                //Validating the CardDetails Model.
                CardValidation cardValidation = new CardValidation();
                ValidationResult cardValidationResult = cardValidation.Validate(payment?.CardDetails);
                if (!cardValidationResult.IsValid)
                    throw new HttpRequestException(cardValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage,
                        new Exception(cardValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage),
                        HttpStatusCode.BadRequest);

                //Validating the Expiry Date Model.
                ExpiryDateValidation expDateValidation = new ExpiryDateValidation();
                ValidationResult expDateValidationResult = expDateValidation.Validate(payment?.CardDetails?.ExpiryDate);
                if (!expDateValidationResult.IsValid)
                    throw new HttpRequestException(expDateValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage,
                        new Exception(expDateValidationResult?.Errors?.FirstOrDefault()?.ErrorMessage),
                        HttpStatusCode.BadRequest);

                await _paymentRepository.PostAsync(payment);
                paymentResponse = _mapper.Map<MakePaymentResponse>(payment);

                await _dispatcher.DispatchAsync(new MakePaymentEvent(payment));
                _logger.LogInformation($"Payment started processing to {request.MerchantId} by {CardMaskService.CardMask(request.CardDetails.Number)} for {request.Amount} {request.CurrencyCode}");
            }
            catch (Exception ex)
            {
                // Validation messages from payment creation will be passed here
                paymentResponse.StatusCode = $"Payment failed:: {ex.Message}";
                _logger.LogError($"Payment failed to {request.MerchantId} by {CardMaskService.CardMask(request.CardDetails.Number)} for {request.Amount} {request.CurrencyCode}. {ex.Message}");
            }

            return paymentResponse;
        }
    }
}
