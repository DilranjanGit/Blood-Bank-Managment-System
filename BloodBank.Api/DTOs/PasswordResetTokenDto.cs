
namespace BloodBank.Api.DTOs;
public class PasswordResetTokenDto
{
    public int ResetTokenId { get; set; }

    public int UserId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime? UsedAt { get; set; }

    public DateTime RequestedAt { get; set; }

    public string? RequestedIp { get; set; }

    public string? RequestedUserAgent { get; set; }

    public string? UsedIp { get; set; }

    public string? UsedUserAgent { get; set; }
}

public class ForgotDto
{
    public string Email { get; set; }
}
public class ResetDto
{
    public string Token { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; } 
}
