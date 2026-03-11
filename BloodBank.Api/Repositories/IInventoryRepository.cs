
using BloodBank.Api.Models;

namespace BloodBank.Api.Repositories;

public interface IInventoryRepository
{
    Task<IEnumerable<Inventory>> GetAllAsync();
    Task<Inventory> GetByIdAsync(int id);
    Task<Inventory> GetByProductGroupAsync(int productId, string bloodGroup);
    Task<Inventory> AddAsync(Inventory entity);
    Task<Inventory> UpdateAsync(Inventory entity);
    Task<bool> DeleteAsync(int id);
}