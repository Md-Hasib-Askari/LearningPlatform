using LearningPlatform.Data.Domain.Enums;
using Microsoft.IdentityModel.Tokens;

namespace LearningPlatform.Business.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, RoleEnum role);
    // string GenerateRefreshToken();
    SecurityToken? ValidateToken(string token);
    Guid? GetUserIdFromToken(string token);
}
