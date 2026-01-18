using LearningPlatform.Data.Domain.Enums;

namespace LearningPlatform.Data.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task UpdateUserRoleAsync(Guid userId, RoleEnum newRole, CancellationToken cancellationToken = default);
}