namespace PaymentGateway.Domain.Models
{
    public class ExpiryDate
    {
        public int Year { get; }
        public int Month { get; }

        public ExpiryDate(int month, int year)
        {
            Year = year;
            Month = month;
        }
    }
}
