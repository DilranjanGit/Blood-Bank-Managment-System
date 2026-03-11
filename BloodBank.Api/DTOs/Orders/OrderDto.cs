
namespace BloodBank.Api.DTOs.Orders;

public class OrderDto
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string BloodGroup { get; set; }
    public int Quantity { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string DeliveryAddress { get; set; }
    public decimal TotalAmount { get; set; }
    public string OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }

}



