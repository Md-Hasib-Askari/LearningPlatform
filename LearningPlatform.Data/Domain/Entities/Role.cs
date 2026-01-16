using System.ComponentModel.DataAnnotations;

public class Role : BaseEntity, IAuditableEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; private set; } = null!;

    [MaxLength(250)]
    public string Description { get; private set; } = null!;

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    // Audit
    public DateTime CreatedAt { get; private set; }

    public string? CreatedBy { get; private set; }

    public DateTime? LastModifiedAt { get; private set; }
    public string? LastModifiedBy { get; private set; }

    // EF Core only
    private Role()
    {
        Name = null!;
        Description = null!;
    }

    public Role(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Role Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name is required");

        return new Role(name, description);
    }
}