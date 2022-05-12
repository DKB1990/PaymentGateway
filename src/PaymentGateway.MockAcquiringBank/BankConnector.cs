using System.Threading.Tasks;
using PaymentGateway.Domain.Enums;
using PaymentGateway.SimulatorBank.Models;

namespace PaymentGateway.SimulatorBank
{
    public class BankConnector : IBankConnector
    {
        /// <summary>
        /// Process a payment through a mock aquiring bank
        /// </summary>
        /// <param name="request">the payment request</param>
        /// <returns>the payment response from the bank</returns>
        public async Task<BankPaymentResponse> ProcessPaymentAsync(BankPaymentRequest request)
        {
            // DECLINED Payment because of INSUFFIENT FUNDS
            if (request.Amount > 10000)
                return await Task.FromResult(new BankPaymentResponse()
                {
                    StatusCode = PaymentStatusCode.Declined,
                    PaymentId = request.PaymentId,
                    MerchantId = request.MerchantId,
                    DeclinedReasonCode = PaymentDeclinedReasonCode.InsufficientFunds
                });

            //Fradulent Payment
            else if (request.CardDetails.CVV.Equals("999") || request.Description.Contains("Spam"))
                return await Task.FromResult(new BankPaymentResponse()
                {
                    StatusCode = PaymentStatusCode.Declined,
                    PaymentId = request.PaymentId,
                    MerchantId = request.MerchantId,
                    DeclinedReasonCode = PaymentDeclinedReasonCode.PotentialFraudulentPayment
                });

            //Invalid Card Details
            else if (request.CardDetails.BeneficiaryName.Length < 3)
                return await Task.FromResult(new BankPaymentResponse()
                {
                    StatusCode = PaymentStatusCode.Declined,
                    PaymentId = request.PaymentId,
                    MerchantId = request.MerchantId,
                    DeclinedReasonCode = PaymentDeclinedReasonCode.InvalidCardDetails
                });

            // Timeout
            else if (request.CardDetails.Number.Equals("5555444433332222"))
                return await Task.FromResult(new BankPaymentResponse()
                {
                    StatusCode = PaymentStatusCode.Declined,
                    PaymentId = request.PaymentId,
                    MerchantId = request.MerchantId,
                    DeclinedReasonCode = PaymentDeclinedReasonCode.Timeout
                });

            //SUCCESS
            return await Task.FromResult(new BankPaymentResponse()
            {
                StatusCode = PaymentStatusCode.Approved,
                PaymentId = request.PaymentId,
                MerchantId = request.MerchantId
            });
        }
    }
}
