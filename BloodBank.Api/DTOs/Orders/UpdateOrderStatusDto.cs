namespace BloodBank.Api.DTOs.Orders;


public class UpdateOrderStatusDto
{
    public string Status { get; set; } // Confirmed, Delivered, Cancelled, etc.
    public int UpdatedBy { get; set; } // UserId (staff/admin)
    public string Note { get; set; }   // optional
}
