using Microsoft.EntityFrameworkCore;
using BloodBank.Api.Security;
using BloodBank.Api.Models;
using BloodBank.Api.Services;
using BloodBank.Api.Repositories;
using System.Security.Cryptography;

public interface IPasswordResetService
{
    Task RequestAsync(string email, string originBaseUrl, string ip, string userAgent);
    Task<bool> ResetAsync(string rawToken, string newPassword, string ip, string userAgent);
}

public class PasswordResetService : IPasswordResetService
{
    private readonly IUserRepository _ctx;
    private readonly IEmailService _mailer;
    private readonly IConfiguration _cfg;

    public PasswordResetService(IUserRepository ctx, IEmailService mailer, IConfiguration cfg)
    {
        _ctx = ctx;
        _mailer = mailer;
        _cfg = cfg;
    }

    public async Task RequestAsync(string email, string originBaseUrl, string ip, string userAgent)
    {
        // Look up user silently (avoid enumeration leakage)
        var user = await _ctx.GetUserByEmail(email);

        // Always behave the same way even if not found
        if (user != null)
        {
            // (1) Create token blob
            var (raw, hash, salt) = ResetToken.Create();

            // (2) Store token row: 15-30 minutes TTL
            var entity = new PasswordResetToken
            {
                UserId = user.UserId,
                TokenHash = hash,
                TokenSalt = salt,
                ExpiresAt = DateTime.UtcNow.AddMinutes(20),
                RequestedIp = ip,
                RequestedUserAgent = userAgent
            };
            await _ctx.AddPasswordResetToken(entity);
            

            // (3) Build link for your UI (Angular route or API)
            // Prefer a FRONTEND route: e.g., https://app.yourdomain.com/reset-password?token=...
            // If you only have API now, expose a POST /reset-password that accepts {token,newPassword}
            var link = $"{originBaseUrl.TrimEnd('/')}/reset-password?token={raw}";

            // (4) Send email
            var subject = "Password reset request";
            var html = $@"<p>You (or someone) requested a password reset.</p>
                          <p><a href=""{link}"">Click here to reset your password</a>. 
                          This link expires in 20 minutes.</p>
                          <p>If you didn’t request this, you can ignore this message.</p>";
            await _mailer.SendAsync(user.Email, subject, html);
        }

        // Return silently (generic response handled by Controller)
    }

    public async Task<bool> ResetAsync(string rawToken, string newPassword, string ip, string userAgent)
    {
        // Find matching token by hash
        // We must compute hash with stored salt and compare in constant time
        var candidateTokens = await _ctx.GetPasswordResetToken();
            
        foreach (var row in candidateTokens)
        {
            var hash = ResetToken.HashFromRaw(rawToken, row.TokenSalt);
            if (CryptographicOperations.FixedTimeEquals(hash, row.TokenHash))
            {
                // Token matched; load user
                var user = await _ctx.GetUserById(row.UserId);
                if (user == null) return false;

                // Update password
               // var (ok, err) = PasswordRules.Validate(newPassword);
               // if (!ok) throw new InvalidOperationException(err);

                user.PasswordHash = PasswordHasher.HashPassword(newPassword);

                // Mark token used and optionally invalidate all other outstanding tokens
                row.UsedAt = DateTime.UtcNow;
                row.UsedIp = ip;
                row.UsedUserAgent = userAgent;

                // Optional hardening: invalidate other tokens for this user
               /* var others = await _ctx.GetPasswordResetToken().ToList()
                    .Where(t => t.UserId == row.UserId && t.UsedAt == null && t.ExpiresAt > DateTime.UtcNow && t.ResetTokenId != row.ResetTokenId)
                    .ToListAsync();
                foreach (var o in others) o.ExpiresAt = DateTime.UtcNow; */
                await _ctx.UpdateUser(user);
                await _ctx.UpdatePasswordResetToken(row); 

                // Notify user that password changed (no password in email)
                try
                {
                    await _mailer.SendAsync(user.Email, "Your password was reset",
                        "<p>Your password has just been changed. If this wasn't you, contact support immediately.</p>");
                } catch { // do not fail the reset on email issues  
                }

                return true;
            }
        }
        return false;
    }
    }
    
