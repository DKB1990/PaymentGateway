﻿namespace PaymentGateway.Domain.Enums
{
    /// <summary>
    /// Source: https://docs.checkout.com/resources/codes/response-codes
    /// </summary>
    public enum PaymentDeclinedReasonCode
    {
        /// <summary>
        /// The payment request has timed out
        /// </summary>
        Timeout,

        /// <summary>
        /// The aquiring (Mock) bank is unavailable
        /// </summary>
        BankUnavailable,

        /// <summary>
        /// The API gateway encountered a internal error
        /// </summary>
        GatewayError,

        /// <summary>
        /// The merchant provided is not registered
        /// </summary>
        InvalidMerchant,

        /// <summary>
        /// The card details provided are invalid
        /// </summary>
        InvalidCardDetails,

        /// <summary>
        /// Insufficient funds in bank account to perform transaction
        /// </summary>
        InsufficientFunds,

        /// <summary>
        /// A payment that is potentially Fradulent
        /// </summary>
        PotentialFraudulentPayment
    }
}
