using System.Net;
using System.Net.Mail;

public interface ISmtpClient
{
    NetworkCredential Credentials { get; set; }
    bool EnableSsl { get; set; }
    Task SendMailAsync(MailMessage mailMessage, CancellationToken cancellationToken = default);
}