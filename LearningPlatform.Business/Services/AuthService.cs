using Microsoft.Extensions.Configuration;
using TaskManagement.Business.Interfaces;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(forgotPasswordDto.Email, cancellationToken);
        if (user == null)
        {
            throw new InvalidOperationException("User with this email does not exist.");
        }

        // Simulate sending email
        // TODO: Integrate with an email service
        var resetToken = Guid.NewGuid().ToString();
        Console.WriteLine($"Password reset token for {forgotPasswordDto.Email}: {resetToken}");

        user.SetPasswordResetToken(resetToken, DateTime.UtcNow.AddHours(1));
        await _userRepository.UpdateAsync(user, cancellationToken);
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
}