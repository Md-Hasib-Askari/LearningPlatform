using System.ComponentModel.DataAnnotations;

public sealed class UpdateUserDto
{
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = null!;

    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = null!;
}