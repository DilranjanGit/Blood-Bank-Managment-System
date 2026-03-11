using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class BloodDonation
{
    public int DonationId { get; set; }

    public int DonorId { get; set; }

    public DateTime DonationDate { get; set; }

    public decimal? Rbccount { get; set; }

    public decimal? Wbccount { get; set; }

    public decimal? PlateletsCount { get; set; }

    public int VolumeInMl { get; set; }

    public string Status { get; set; } = null!;

    public virtual Donor Donor { get; set; } = null!;
}
