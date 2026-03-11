using BloodBank.Api.Models;
using Microsoft.EntityFrameworkCore;


namespace BloodBank.Api.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly BloodBankContext _context;

    public InventoryRepository(BloodBankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Inventory>> GetAllAsync()
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .OrderBy(i => i.ProductId)
            .ThenBy(i => i.BloodGroup)
            .ToListAsync();
    }

    public async Task<Inventory> GetByIdAsync(int id)
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.InventoryId == id);
    }

    public async Task<Inventory> GetByProductGroupAsync(int productId, string bloodGroup)
    {
        return await _context.Inventories
            .Include(i => i.Product)
            .FirstOrDefaultAsync(i => i.ProductId == productId && i.BloodGroup == bloodGroup);
    }

    public async Task<Inventory> AddAsync(Inventory entity)
    {
        entity.LastUpdated = DateTime.UtcNow;
        _context.Inventories.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Inventory> UpdateAsync(Inventory entity)
    {
        entity.LastUpdated = DateTime.UtcNow;
        _context.Inventories.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.Inventories.FindAsync(id);
        if (entity == null) return false;

        _context.Inventories.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}