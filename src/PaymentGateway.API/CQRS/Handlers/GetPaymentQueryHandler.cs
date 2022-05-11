using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.API.Queries;
using PaymentGateway.API.Responses;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;

namespace PaymentGateway.API.CQRS.Queries
{
    public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, GetPaymentResponse>
    {
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<GetPaymentHandler> _logger;

        public GetPaymentHandler(ILogger<GetPaymentHandler> logger,
                IPaymentRepository paymentRepository,
                IMapper mapper)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<GetPaymentResponse> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            Payment payment = await _paymentRepository.GetAsync(request.Id);
            _logger.LogInformation($"Payment id {request.Id} :: Merchant id {request.MerchantId}");

            if (payment == null)
                return null;

            GetPaymentResponse paymentResponse = _mapper.Map<GetPaymentResponse>(payment);
            paymentResponse.CardNumber = CardMaskService.CardMask(payment.CardDetails.Number).ToString();
            return paymentResponse;
        }
    }
}

