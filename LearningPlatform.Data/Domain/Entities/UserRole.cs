using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UserRole : BaseEntity, IAuditableEntity
{
    [Required]
    public Guid UserId { get; private set; }

    [Required]
    public Guid RoleId { get; private set; }

    // Navigation Properties
    [ForeignKey(nameof(UserId))]
    public User User { get; private set; } = null!;

    [ForeignKey(nameof(RoleId))]
    public Role Role { get; private set; } = null!;

    // Audit
    public DateTime CreatedAt { get; private set; }

    public string? CreatedBy { get; private set; }

    public DateTime? LastModifiedAt { get; private set; }

    public string? LastModifiedBy { get; private set; }

    // EF Core only
    private UserRole() { }

    public UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }

    public static UserRole Create(Guid userId, Guid roleId)
    {
        return new UserRole(userId, roleId);
    }
}