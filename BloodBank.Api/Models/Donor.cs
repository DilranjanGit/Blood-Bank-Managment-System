using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class Donor
{
    public int DonorId { get; set; }

    public int UserId { get; set; }

    public string BloodGroup { get; set; } = null!;

    public int Age { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<BloodDonation> BloodDonations { get; set; } = new List<BloodDonation>();

    public virtual User User { get; set; } = null!;
}
