using BloodBank.Api.DTOs.Inventory;

namespace BloodBank.Api.Services;

public interface IInventoryService
{
    Task<IEnumerable<InventoryDto>> GetAllAsync();
    Task<InventoryDto> GetByIdAsync(int id);
    Task<InventoryDto> CreateAsync(CreateInventoryDto dto);
    Task<InventoryDto> UpdateAsync(int id, UpdateInventoryDto dto);
    Task<bool> DeleteAsync(int id);
}