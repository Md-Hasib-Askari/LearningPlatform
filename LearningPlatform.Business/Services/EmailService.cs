using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromAddress;

    public EmailService(string smtpHost, int smtpPort, string fromAddress, string smtpUser = "", string smtpPass = "", IConfiguration configuration = null!)
    {
        _smtpClient = new SmtpClient(smtpHost, smtpPort, configuration)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };
        _fromAddress = fromAddress;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var mailMessage = new MailMessage(_fromAddress, to, subject, body)
        {
            IsBodyHtml = true
        };

        await _smtpClient.SendMailAsync(mailMessage, cancellationToken);
    }
}