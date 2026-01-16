using System.Net;
using System.Net.Mail;
using LearningPlatform.Business.Interfaces;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly ISmtpClient _smtpClient;
    private readonly string _fromAddress;

    public EmailService(ISmtpClient smtpClient, IConfiguration configuration)
    {
        // var smtpHost = configuration["Email:SmtpHost"];
        // var smtpPort = int.Parse(configuration["Email:SmtpPort"] ?? "25");
        // var smtpUser = configuration["Email:SmtpUser"];
        // var smtpPass = configuration["Email:SmtpPass"];
        // var fromAddress = configuration["Email:FromAddress"];

        _smtpClient = smtpClient;
        _fromAddress = configuration["Email:FromAddress"]!;

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