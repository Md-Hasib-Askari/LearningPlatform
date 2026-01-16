using System.ComponentModel.DataAnnotations.Schema;
using LearningPlatform.Data.Domain.Enums;

public class Course : BaseEntity, IAuditableEntity
{
    public Guid InstructorId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int DurationInHours { get; private set; }
    public bool IsPublished { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    [ForeignKey(nameof(InstructorId))]
    public User? Instructor { get; private set; }

    public Course(string title, string description, int durationInHours, Guid? instructorId)
    {
        Title = title;
        Description = description;
        DurationInHours = durationInHours;
        InstructorId = instructorId ?? Guid.Empty;
        IsPublished = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void AssignInstructor(Guid instructorId)
    {
        InstructorId = instructorId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish()
    {
        IsPublished = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description, int durationInHours)
    {
        Title = title;
        Description = description;
        DurationInHours = durationInHours;
        UpdatedAt = DateTime.UtcNow;
    }
}