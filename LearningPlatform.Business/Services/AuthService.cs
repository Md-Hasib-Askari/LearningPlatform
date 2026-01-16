using LearningPlatform.Business.Interfaces;
using Microsoft.Extensions.Configuration;
using LearningPlatform.Data.Interfaces;
using LearningPlatform.Data.Domain.Enums;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IEmailService emailService, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetByEmailAsync(email, cancellationToken);
    }

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetByIdAsync(userId, cancellationToken);
    }

    public async Task<RoleEnum> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }
        return user.Role;
    }

    public async Task<string?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByEmailAsync(loginDto.Email, cancellationToken);
        if (user == null)
        {
            return null;
        }

        if (BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role);
            return token;
        }
        return null;
    }

    public async Task<User> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        if (await UserExistsAsync(registerDto.Email, cancellationToken))
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var passwordSettings = _configuration.GetSection("passwordSettings");
        var saltRounds = int.Parse(passwordSettings["SaltRounds"] ?? "10");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password1, saltRounds);

        var userToCreate = User.Create(
            registerDto.Email,
            hashedPassword,
            registerDto.FirstName,
            registerDto.LastName
        );
        await _userRepository.AddAsync(userToCreate, cancellationToken);
        return userToCreate;
    }

    public async Task UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        var passwordSettings = _configuration.GetSection("passwordSettings");
        var saltRounds = int.Parse(passwordSettings["SaltRounds"] ?? "10");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password1, saltRounds);

        var updatedUser = User.Update(user, updateUserDto.FirstName, updateUserDto.LastName, hashedPassword);
        await _userRepository.UpdateAsync(updatedUser, cancellationToken);
    }

    public async Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return user != null;
    }

    public bool VerifyUserAsync(string token, CancellationToken cancellationToken = default)
    {
        var userId = _jwtService.GetUserIdFromToken(token);
        return userId != null;
    }

    public async Task SendForgotPasswordTokenAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User with this email does not exist.");
        }

        var resetToken = Random.Shared.Next(100000, 999999).ToString();

        // Set token and expiry on user entity
        user.SetPasswordResetToken(resetToken, DateTime.UtcNow.AddHours(1));
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Send email with reset token
        await _emailService.SendEmailAsync(
            forgotPasswordDto.Email,
            "Password Reset Request",
            $"Use this token to reset your password: {resetToken}",
            cancellationToken
        );
    }

    public async Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(resetPasswordDto.Email, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User with this email does not exist.");
        }

        if (user.PasswordResetTokenExpiry == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Password reset token has expired.");
        }

        if (user.PasswordResetToken != resetPasswordDto.Token)
        {
            throw new InvalidOperationException("Invalid password reset token.");
        }

        var passwordSettings = _configuration.GetSection("passwordSettings");
        var saltRounds = int.Parse(passwordSettings["SaltRounds"] ?? "12");
        var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword1, saltRounds);

        // Clear reset token and expiry
        user.SetPasswordHash(hashedNewPassword);
        user.ClearPasswordResetToken();
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}