namespace BloodBank.Api.DTOs.Orders;



public class CreateOrderDto
{
   // public int UserId { get; set; }
    //public string OrderNumber { get; set; }
    public int ProductId { get; set; }
    public string BloodGroup { get; set; } // e.g., "A+", "O-"
    public int Quantity { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string DeliveryAddress { get; set; }
    public decimal TotalAmount { get; set; } // Assuming pricing is pre-calculated or set elsewhere
}
