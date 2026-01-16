using System.Security.Claims;

namespace LearningPlatform.Business.Interfaces;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email, IEnumerable<string> roles);
    // string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
    Guid? GetUserIdFromToken(string token);
}
