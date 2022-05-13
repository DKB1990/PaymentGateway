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
                        ExpiryDate = new Domain.Models.ExpiryDate(2022, 2),
                        BeneficiaryName = request.CardDetails.BeneficiaryName,
                    }
                };

                //Validating the Model.
                PaymentValidation paymentValidation = new PaymentValidation();
                ValidationResult result= paymentValidation.Validate(payment);
                if (!result.IsValid)
                    throw new HttpRequestException(result?.Errors?.FirstOrDefault()?.ErrorMessage,
                        new Exception(result?.Errors?.FirstOrDefault()?.ErrorMessage),
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
