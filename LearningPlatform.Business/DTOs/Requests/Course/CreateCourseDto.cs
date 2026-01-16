using System.ComponentModel.DataAnnotations;

public sealed class CreateCourseDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int DurationInHours { get; set; }

    public Guid InstructorId { get; set; }
}