public interface IUserService
{
    Task<UserDto> UpdateUserAsync(Guid userId, UpdateUserDto updateUserDto, CancellationToken cancellationToken);
    Task UpdateUserPasswordAsync(Guid userId, UpdatePasswordDto updatePasswordDto, CancellationToken cancellationToken);
    Task<UserDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken);
}