using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Models;

namespace PaymentGateway.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMemoryCache _cache;

        public PaymentRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<Payment> GetAsync(Guid id)
        {
            bool isPaymentAvailable = _cache.TryGetValue<Payment>(id, out var payment);
            if (isPaymentAvailable)
                return await Task.FromResult(payment);

            return null;
        }

        public async Task PostAsync(Payment payment)
        {
            var doesExistAlready = _cache.TryGetValue<Payment>(payment.Id, out var existingPayment);
            if (doesExistAlready)
                throw new ArgumentException($"Duplicate Payment Id :: {payment.Id}");

           await Task.Run(() =>_cache.Set(payment.Id, payment));
        }

        public async Task UpdateAsync(Payment payment)
        {
            if (!_cache.TryGetValue<Payment>(payment.Id, out var existingPayment))
                throw new ArgumentException($"Payment NOT found with id {payment.Id}");

            await Task.Run(() => _cache.Set(payment.Id, payment));
        }
    }
}
