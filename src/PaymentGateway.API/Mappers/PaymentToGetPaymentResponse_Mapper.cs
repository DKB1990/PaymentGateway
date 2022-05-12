using AutoMapper;
using PaymentGateway.API.Responses;
using PaymentGateway.Domain.Enums;
using PaymentGateway.Domain.Models;
using PaymentGateway.Domain.Services;

namespace PaymentGateway.API.Mappers
{
    public class PaymentToGetPaymentResponse_Mapper : Profile
    {
        public PaymentToGetPaymentResponse_Mapper() => CreateMap<Payment, GetPaymentResponse>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.StatusCode == PaymentStatusCode.Declined ? src.StatusCode.ToString() + " - " + src.DeclinedReasonCode.ToString() : src.StatusCode.ToString()))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.CardNumber, opt => opt.MapFrom(src => CardMaskService.CardMask(src.CardDetails.Number)))
              .ForMember(dest => dest.CardExpiryDate, opt => opt.MapFrom(src =>
                $"{src.CardDetails.ExpiryDate.Month}-{src.CardDetails.ExpiryDate.Year}"))
              .ForMember(dest => dest.BeneficiaryName, opt => opt.MapFrom(src => src.CardDetails.BeneficiaryName))
              .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
              .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode));
    }
}
