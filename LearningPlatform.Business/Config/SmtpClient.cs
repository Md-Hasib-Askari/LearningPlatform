using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class SmtpClient : ISmtpClient
{
    public NetworkCredential Credentials { get; set; } = null!;
    public bool EnableSsl { get; set; }

    public Task SendMailAsync(MailMessage mailMessage, CancellationToken cancellationToken = default)
    {
        // Implementation details...
        return Task.CompletedTask;
    }
}