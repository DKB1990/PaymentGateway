using AutoMapper;
using PaymentGateway.Domain.Models;
using PaymentGateway.SimulatorBank.Models;

namespace PaymentGateway.API.Mappers
{
    public class PaymentToBankPaymentRequest_Mapper : Profile
    {
        public PaymentToBankPaymentRequest_Mapper()
        {
            CreateMap<Payment, BankPaymentRequest>()
                .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.MerchantId, opt => opt.MapFrom(src => src.MerchatId.ToString()))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CurrencyCode, opt => opt.MapFrom(src => src.CurrencyCode))
                .ForMember(dest => dest.CardDetails, opt => opt.MapFrom(src => new CardDetail
                {
                    CVV = src.CardDetails.CVV,
                    Number = src.CardDetails.Number,
                    ExpiryDate = new CardExpiryDate(src.CardDetails.ExpiryDate.Year, src.CardDetails.ExpiryDate.Month),
                    BeneficiaryName = src.CardDetails.BeneficiaryName,
                }));
        }
    }
}