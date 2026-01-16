using System.ComponentModel.DataAnnotations;

public sealed class RegisterDto()
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(8)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain upper, lower, and number."
    )]
    public string Password1 { get; set; } = null!;

    [Required]
    [Compare("Password1", ErrorMessage = "Passwords do not match.")]
    public string Password2 { get; set; } = null!;
}