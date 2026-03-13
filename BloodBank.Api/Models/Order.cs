using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class Order
{
    public int OrderId { get; set; }
    
    public string OrderNumber { get; set; }
    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public string BloodGroup { get; set; } = null!;

    public DateTime DeliveryDate { get; set; }

    public string DeliveryAddress { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<OrderStatusHistory> OrderStatusHistories { get; set; } = new List<OrderStatusHistory>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual BloodProduct Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
