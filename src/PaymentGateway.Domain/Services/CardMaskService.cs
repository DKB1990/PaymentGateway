using System.Text;

namespace PaymentGateway.Domain.Services
{
    public static class CardMaskService
    {
        private const int MASKING_FROM_INDEX = 4;
        private const int MASKING_TILL_INDEX = 12;

        public static string CardMask(string cardNumber)
        {
            StringBuilder maskedCardNumber = new StringBuilder();
            maskedCardNumber.Append(cardNumber.Substring(0, MASKING_FROM_INDEX));
            maskedCardNumber.Append(new string('*', MASKING_TILL_INDEX - MASKING_FROM_INDEX));
            maskedCardNumber.Append(cardNumber.Substring(MASKING_TILL_INDEX, cardNumber.Length - MASKING_TILL_INDEX));
            return maskedCardNumber.ToString();
        }
    }
}
