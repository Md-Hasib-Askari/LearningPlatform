using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using LearningPlatform.Business.Interfaces;
using LearningPlatform.Data.Domain.Enums;
using Microsoft.Extensions.Logging;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<JwtService> _logger;

    public JwtService(IOptions<JwtSettings> jwtSettings, ILogger<JwtService> logger)
    {
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public string GenerateToken(Guid userId, string email, RoleEnum role)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public Guid? GetUserIdFromToken(string token)
    {
        try
        {
            var validatedToken = ValidateToken(token);
            if (validatedToken != null)
            {
                var jwtToken = validatedToken as JwtSecurityToken;
                var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("User ID claim not found in token.");
                    return null;
                }

                if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    _logger.LogWarning("Invalid User ID format in token.");
                    return null;
                }
                return userId;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Failed to get user ID from token: {ExceptionMessage}", ex.Message);
            // Token validation failed
            return null;
        }

        return null;
    }

    public SecurityToken? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);

            // _logger.LogInformation("Token validated successfully: {ValidatedToken}", validatedToken);

            return validatedToken;
        }
        catch (Exception ex)
        {
            _logger.LogWarning("Token validation failed: {ExceptionMessage}", ex.Message);
            // Token validation failed
            return null;
        }
    }
}