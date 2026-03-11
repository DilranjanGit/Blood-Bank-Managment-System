using AutoMapper;
using BloodBank.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace BloodBank.Api.Repositories;

// Repositories/OrderRepository.cs
public class OrderRepository : IOrderRepository
{
    private readonly BloodBankContext _ctx;
    public OrderRepository(BloodBankContext ctx) => _ctx = ctx;

    public async Task<Order> GetByIdAsync(int id, bool includeHistory = false)
    {
        IQueryable<Order> q = _ctx.Orders;
        if (includeHistory)
            q = q.Include(o => o.OrderStatusHistories);
        return await q.FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<IEnumerable<Order>> GetByUserAsync(int userId) =>
        await _ctx.Orders.Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

    public async Task<IEnumerable<Order>> GetAllAsync() =>
        await _ctx.Orders.OrderByDescending(o => o.CreatedAt).ToListAsync();

    public async Task<Order> AddAsync(Order order)
    {
        _ctx.Orders.Add(order);
        await _ctx.SaveChangesAsync();
        return order;
    }

    public async Task UpdateAsync(Order order)
    {
        _ctx.Orders.Update(order);
        await _ctx.SaveChangesAsync();
    }

    public async Task AddStatusHistoryAsync(OrderStatusHistory history)
    {
        _ctx.OrderStatusHistories.Add(history);
        await _ctx.SaveChangesAsync();
    }

    public Task SaveChangesAsync() => _ctx.SaveChangesAsync();
}