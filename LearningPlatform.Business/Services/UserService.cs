using LearningPlatform.Data.Interfaces;
using Microsoft.Extensions.Configuration;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _configuration;
    public UserService(IUserRepository userRepo, IConfiguration configuration)
    {
        _userRepo = userRepo;
        _configuration = configuration;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString()
        };
    }

    public async Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.SetName(updateUserDto.FirstName, updateUserDto.LastName);

        await _userRepo.UpdateAsync(user, cancellationToken);
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    }

    public async Task UpdateUserPasswordAsync(Guid userId, UpdatePasswordDto updatePasswordDto, CancellationToken cancellationToken)
    {
        var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (updatePasswordDto.CurrentPassword != null)
        {
            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(updatePasswordDto.CurrentPassword, user.PasswordHash))
            {
                throw new Exception("Current password is incorrect");
            }
        }

        var saltRounds = int.Parse(_configuration["passwordSettings:SaltRounds"] ?? "10");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updatePasswordDto.NewPassword1, saltRounds);

        user.SetPasswordHash(hashedPassword);
        await _userRepo.UpdateAsync(user, cancellationToken);
    }
}