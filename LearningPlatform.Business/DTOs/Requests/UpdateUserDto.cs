using System.ComponentModel.DataAnnotations;

public sealed class UpdateUserDto()
{
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = null!;

    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = null!;

    [MinLength(8)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain upper, lower, and number."
    )]
    public string Password1 { get; set; } = null!;

    [Compare("Password1", ErrorMessage = "Passwords do not match.")]
    public string Password2 { get; set; } = null!;
}