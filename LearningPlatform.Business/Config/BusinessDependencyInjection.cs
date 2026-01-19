using LearningPlatform.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class BusinessDependencyInjection
{
    public static IServiceCollection AddBusinessServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // Register business services here
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IEnrollmentService, EnrollmentService>();

        // Email Service Registration
        var emailSettings = configuration.GetSection("Email");
        EmailSettings emailConfig = new EmailSettings
        {
            SmtpHost = emailSettings["SmtpHost"] ?? "smtp.gmail.com",
            Port = int.Parse(emailSettings["Port"] ?? "587"),
            ApiKey = emailSettings["ApiKey"] ?? "",
            FromAddress = emailSettings["FromAddress"] ?? ""
        };
        services.AddScoped<IEmailService, EmailService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<EmailService>>();
            return new EmailService(emailConfig, logger);
        });

        // Other service registrations...
        // var emailConfig = configuration.GetSection("Email");

        return services;
    }
}