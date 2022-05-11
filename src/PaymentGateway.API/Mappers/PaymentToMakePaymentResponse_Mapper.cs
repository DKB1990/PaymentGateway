using AutoMapper;
using PaymentGateway.API.Responses;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.API.Mappers
{
    public class PaymentToMakePaymentResponse_Mapper : Profile
    {
        public PaymentToMakePaymentResponse_Mapper()
        {
            CreateMap<Payment, MakePaymentResponse>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => "Payment processing"));
        }
    }
}
