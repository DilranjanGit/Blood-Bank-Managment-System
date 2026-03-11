namespace BloodBank.Api.DTOs.Orders;


public class UpdateOrderDeliveryDto
{
    public DateTime DeliveryDate { get; set; }
    public string DeliveryAddress { get; set; }
}
