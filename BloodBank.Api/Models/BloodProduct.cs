using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class BloodProduct
{
    public int ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
