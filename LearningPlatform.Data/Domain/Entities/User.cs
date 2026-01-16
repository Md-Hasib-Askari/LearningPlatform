using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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

    public string PasswordResetToken { get; private set; } = null!;
    public DateTime? PasswordResetTokenExpiry { get; private set; }

    private readonly List<UserRole> _userRoles = new();
    public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

    // Audit
    public DateTime CreatedAt { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
    public string? LastModifiedBy { get; private set; }

    // Factory Method
    public static User Create(string email, string passwordHash, string firstName, string lastName)
    {
        var user = new User();
        user.SetName(firstName, lastName);
        user.SetPasswordHash(passwordHash);
        user.Email = email;
        user.IsActive = true;
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

    public void AssignRole(Role role)
    {
        if (role == null)
            throw new DomainException("Role cannot be null");

        if (_userRoles.Any(ur => ur.RoleId == role.Id))
            throw new DomainException("User already has this role assigned");

        _userRoles.Add(UserRole.Create(this.Id, role.Id));
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
