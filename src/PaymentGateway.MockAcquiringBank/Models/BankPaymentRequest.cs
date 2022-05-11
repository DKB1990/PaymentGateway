namespace PaymentGateway.SimulatorBank.Models
{
    public class BankPaymentRequest
    {
        public string PaymentId { get; set; }
        public string MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public CardDetail CardDetails { get; set; }
    }

    public class CardDetail
    {
        public string BeneficiaryName { get; set; }
        public string CVV { get; set; }
        public string Number { get; set; }
        public CardExpiryDate ExpiryDate { get; set; }
    }

    public class CardExpiryDate
    {
        public CardExpiryDate(int year, int month)
        {
            Year = year;
            Month = month;
        }

        public int Month { get; }
        public int Year { get; }
    }
}
