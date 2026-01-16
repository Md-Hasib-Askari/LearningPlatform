using LearningPlatform.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class ServiceDependencyInjection
{
    public static IServiceCollection AddBusinessServices(
        this IServiceCollection services)
    {
        // Register business services here
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();

        // Email Service Registration
        services.AddScoped<ISmtpClient, SmtpClient>();
        services.AddScoped<IEmailService, EmailService>();

        // Other service registrations...
        // var emailConfig = configuration.GetSection("Email");

        return services;
    }
}