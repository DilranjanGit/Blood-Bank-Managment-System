using BloodBank.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodBank.Api.Repositories;

public interface IInventoryQueries
{
    Task<Inventory> GetInventoryAsync(int productId, string bloodGroup);
}
