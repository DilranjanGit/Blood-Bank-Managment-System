using BloodBank.Api.Models;

namespace BloodBank.Api.Repositories;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(int id, bool includeHistory = false);
    Task<IEnumerable<Order>> GetByUserAsync(int userId);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task AddStatusHistoryAsync(OrderStatusHistory history);
    Task SaveChangesAsync();
}