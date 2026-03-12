using System;
using System.Collections.Generic;

namespace BloodBank.Api.Models;

public partial class PasswordResetToken
{
    public int ResetTokenId { get; set; }

    public int UserId { get; set; }

    public byte[] TokenHash { get; set; } = null!;

    public byte[] TokenSalt { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime RequestedAt { get; set; }

    public string? RequestedIp { get; set; }

    public string? RequestedUserAgent { get; set; }

    public string? UsedIp { get; set; }

    public string? UsedUserAgent { get; set; }
}
