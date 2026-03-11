
namespace BloodBank.Api.DTOs.Inventory;

public class InventoryDto
{
    public int InventoryId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string BloodGroup { get; set; }
    public int QuantityAvailable { get; set; }
    public DateTime LastUpdated { get; set; }
}