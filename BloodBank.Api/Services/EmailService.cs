using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace BloodBank.Api.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _cfg;
    public EmailService(IConfiguration cfg) => _cfg = cfg;

    public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct = default)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_cfg["Smtp:From"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_cfg["Smtp:Server"], int.Parse(_cfg["Smtp:Port"]), SecureSocketOptions.StartTls, ct);
        await smtp.AuthenticateAsync(_cfg["Smtp:User"], _cfg["Smtp:Password"], ct);
        await smtp.SendAsync(message, ct);
        await smtp.DisconnectAsync(true, ct);
    }
}