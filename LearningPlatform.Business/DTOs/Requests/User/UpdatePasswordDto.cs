using System.ComponentModel.DataAnnotations;

public sealed class UpdatePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; } = null!;

    [MinLength(8)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
        ErrorMessage = "Password must contain upper, lower, and number."
    )]
    public string NewPassword1 { get; set; } = null!;

    [Compare("NewPassword1", ErrorMessage = "Passwords do not match.")]
    public string NewPassword2 { get; set; } = null!;
}