using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int OrderId { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public string? PaymentGateway { get; set; }

    public string? TransactionId { get; set; }

    public decimal AmountPaid { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public virtual Order Order { get; set; } = null!;
}
