using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using LearningPlatform.Data.Domain.Enums;

public class User : BaseEntity, IAuditableEntity
{
    [Required]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
    public string Email { get; private set; } = null!;

    [Required]
    [MinLength(8)]
    public string PasswordHash { get; private set; } = null!;

    [Required]
    [MaxLength(50)]
    [MinLength(2)]
    public string FirstName { get; private set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; private set; } = null!;

    [Required]
    public bool IsActive { get; private set; }

    public string PasswordResetToken { get; private set; } = string.Empty;
    public DateTime? PasswordResetTokenExpiry { get; private set; }

    public RoleEnum Role { get; private set; } = RoleEnum.Guest;

    // Audit
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Factory Method
    public static User Create(string email, string passwordHash, string firstName, string lastName)
    {
        var user = new User();
        user.SetName(firstName, lastName);
        user.SetPasswordHash(passwordHash);
        user.Email = email;
        user.IsActive = true;
        user.AssignRole(RoleEnum.Guest);
        user.CreatedAt = DateTime.UtcNow;
        return user;
    }

    public static User Update(User user, string firstName, string lastName, string passwordHash)
    {
        user.SetName(firstName, lastName);
        user.SetPasswordHash(passwordHash);
        user.UpdatedAt = DateTime.UtcNow;
        return user;
    }

    public void AssignRole(RoleEnum role)
    {
        Role = role;
    }

    // Behavioral Methods
    public void SetName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void SetPasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
    }

    public void SetPasswordResetToken(string token, DateTime? expiry)
    {
        PasswordResetToken = token;
        PasswordResetTokenExpiry = expiry;
    }

    public void ClearPasswordResetToken()
    {
        PasswordResetToken = string.Empty;
        PasswordResetTokenExpiry = null;
    }
}
