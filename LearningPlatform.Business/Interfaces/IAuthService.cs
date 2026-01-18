using LearningPlatform.Data.Domain.Enums;

namespace LearningPlatform.Business.Interfaces;

public interface IAuthService
{
    Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<User> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);
    Task<string?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    bool VerifyUserAsync(string token, CancellationToken cancellationToken = default);
    Task<RoleEnum> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task SendForgotPasswordTokenAsync(ForgotPasswordDto forgotPasswordDto, CancellationToken cancellationToken = default);
    Task ResetPasswordAsync(ResetPasswordDto resetPasswordDto, CancellationToken cancellationToken = default);
    Task ChangeUserRoleAsync(ChangeUserRoleDto changeUserRoleDto, CancellationToken cancellationToken = default);
}
