

using BloodBank.Api.Models;

namespace BloodBank.Api.Repositories;

public interface IPaymentRepository
{
    Task<Payment> GetByIdAsync(int id);
    Task<Payment> GetByTransactionAsync(string transactionId);
    Task<IEnumerable<Payment>> GetByOrderAsync(int orderId);
    Task<Payment> AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
}
