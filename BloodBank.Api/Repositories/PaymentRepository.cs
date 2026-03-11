
using BloodBank.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Api.Repositories;


public class PaymentRepository : IPaymentRepository
{
    private readonly BloodBankContext _ctx;
    public PaymentRepository(BloodBankContext ctx) => _ctx = ctx;

    public Task<Payment> GetByIdAsync(int id) =>
        _ctx.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);

    public Task<Payment> GetByTransactionAsync(string transactionId) =>
        _ctx.Payments.FirstOrDefaultAsync(p => p.TransactionId == transactionId);

    public async Task<IEnumerable<Payment>> GetByOrderAsync(int orderId) =>
        await _ctx.Payments.Where(p => p.OrderId == orderId)
                           .OrderByDescending(p => p.PaymentDate).ToListAsync();

    public async Task<Payment> AddAsync(Payment payment)
    {
        _ctx.Payments.Add(payment);
        await _ctx.SaveChangesAsync();
        return payment;
    }

    public Task UpdateAsync(Payment payment)
    {
        _ctx.Payments.Update(payment);
        return _ctx.SaveChangesAsync();
    }
}