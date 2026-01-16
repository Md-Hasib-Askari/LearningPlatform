using Microsoft.Extensions.Configuration;
using TaskManagement.Business.Interfaces;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IEmailService emailService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _emailService = emailService;
        _configuration = configuration;
    }

    public Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetByEmailAsync(email, cancellationToken);
    }

    public Task<User?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _userRepository.GetByIdAsync(userId, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            return Enumerable.Empty<Role>();
        }
        return user.UserRoles.Select(ur => ur.Role).ToList();
    }

    public async Task<User?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await GetUserByEmailAsync(loginDto.Email, cancellationToken);
        if (user == null)
        {
            return null;
        }

        var passwordSettings = _configuration.GetSection("passwordSettings");
        var saltRounds = int.Parse(passwordSettings["SaltRounds"] ?? "10");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.Password, saltRounds);
        if (user.PasswordHash != hashedPassword)
        {
            return null;
        }
        return user;
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

        var resetToken = Guid.NewGuid().ToString();

        var passwordSettings = _configuration.GetSection("passwordSettings");
        var saltRounds = int.Parse(passwordSettings["SaltRounds"] ?? "10");
        var hashedResetToken = BCrypt.Net.BCrypt.HashPassword(resetToken, saltRounds);

        // Set token and expiry on user entity
        user.SetPasswordResetToken(hashedResetToken, DateTime.UtcNow.AddHours(1));
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Send email with reset token
        await _emailService.SendEmailAsync(
            forgotPasswordDto.Email,
            "Password Reset Request",
            $"Use this token to reset your password: {resetToken}",
            cancellationToken
        );
    }

    public async Task ResetPasswordAsync(string email, ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User with this email does not exist.");
        }

        if (user.PasswordResetTokenExpiry == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Password reset token has expired.");
        }

        var isTokenValid = BCrypt.Net.BCrypt.Verify(resetPasswordDto.Token, user.PasswordResetToken);
        if (!isTokenValid)
        {
            throw new InvalidOperationException("Invalid password reset token.");
        }

        var passwordSettings = _configuration.GetSection("passwordSettings");
        var saltRounds = int.Parse(passwordSettings["SaltRounds"] ?? "10");
        var hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword1, saltRounds);

        // Clear reset token and expiry
        user.SetPasswordHash(hashedNewPassword);
        user.SetPasswordResetToken(string.Empty, null);
        await _userRepository.UpdateAsync(user, cancellationToken);
    }
}