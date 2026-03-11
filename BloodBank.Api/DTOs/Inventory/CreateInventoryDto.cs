
namespace BloodBank.Api.DTOs.Inventory;
public class CreateInventoryDto
{
    public int ProductId { get; set; }
    public string BloodGroup { get; set; }
    public int QuantityAvailable { get; set; }
}