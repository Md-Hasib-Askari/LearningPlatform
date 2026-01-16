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
        return user;
    }

    public static User Update(User user, string? firstName, string? lastName, string? passwordHash)
    {
        if (firstName != null && lastName != null)
        {
            user.SetName(firstName, lastName);
        }

        if (passwordHash != null)
        {
            user.SetPasswordHash(passwordHash);
        }

        return user;
    }

    public void AssignRole(RoleEnum role)
    {
        Role = role;
    }

    // Behavioral Methods
    [MemberNotNull(nameof(FirstName), nameof(LastName))]
    public void SetName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name is required");

        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name is required");

        FirstName = firstName;
        LastName = lastName;
    }

    [MemberNotNull(nameof(PasswordHash))]

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash is required");

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
        if (string.IsNullOrWhiteSpace(token))
            throw new DomainException("Password reset token is required");

        PasswordResetToken = token;
        PasswordResetTokenExpiry = expiry;
    }

    public void ClearPasswordResetToken()
    {
        PasswordResetToken = string.Empty;
        PasswordResetTokenExpiry = null;
    }
}
