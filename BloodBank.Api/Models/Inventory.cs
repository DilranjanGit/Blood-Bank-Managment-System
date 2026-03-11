using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public string BloodGroup { get; set; } = null!;

    public int QuantityAvailable { get; set; }

    public DateTime LastUpdated { get; set; }

    public virtual BloodProduct Product { get; set; } = null!;
}
