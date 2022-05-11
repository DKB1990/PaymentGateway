namespace PaymentGateway.Domain.Enums
{
    /// <summary>
    /// Source: https://docs.checkout.com/resources/codes/response-codes
    /// </summary>
    public enum PaymentStatusCode
    {
        /// <summary>
        /// Payment waiting for confirmation
        /// </summary>
        Pending,

        /// <summary>
        /// Payment has been Approved
        /// </summary>
        Approved,

        /// <summary>
        /// Payment has been Declined
        /// </summary>
        Declined
    }
}
