using BloodBank.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Api.Repositories;

public class InventoryQueries : IInventoryQueries
{
    private readonly BloodBankContext _ctx;
    public InventoryQueries(BloodBankContext ctx) => _ctx = ctx;

    public Task<Inventory> GetInventoryAsync(int productId, string bloodGroup) =>
        _ctx.Inventories.FirstOrDefaultAsync(i => i.ProductId == productId && i.BloodGroup == bloodGroup);
}