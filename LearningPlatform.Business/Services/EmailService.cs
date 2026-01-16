using System.Net;
using System.Net.Mail;
using LearningPlatform.Business.Interfaces;
using Microsoft.Extensions.Logging;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;
    private readonly string _fromAddress;
    private readonly ILogger<EmailService> _logger;

    public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
    {
        if (emailSettings == null)
        {
            throw new ArgumentNullException(nameof(emailSettings));
        }
        _fromAddress = emailSettings.FromAddress;
        _smtpClient = new SmtpClient
        {
            Host = emailSettings.SmtpHost,
            Port = emailSettings.Port,
            EnableSsl = true,
            Credentials = new NetworkCredential(emailSettings.FromAddress, emailSettings.ApiKey)
        };

        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        // var mailMessage = new MailMessage(_fromAddress, to, subject, body)
        // {
        //     IsBodyHtml = true
        // };


        try
        {
            // await _smtpClient.SendMailAsync(mailMessage, cancellationToken);
            _logger.LogInformation("From: {From}", _fromAddress);
            _logger.LogInformation("Email sent to {To} with subject {Subject}", to, subject);
            _logger.LogInformation("Email body: {Body}", body);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {To} with subject {Subject}", to, subject);
            throw;
        }
    }
}