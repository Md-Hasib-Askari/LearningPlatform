using System.ComponentModel.DataAnnotations;
using LearningPlatform.Data.Domain.Enums;

public sealed class ChangeUserRoleDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public required string NewRole { get; set; }
}