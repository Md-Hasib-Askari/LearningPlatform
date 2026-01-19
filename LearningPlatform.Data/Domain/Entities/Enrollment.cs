using System.ComponentModel.DataAnnotations.Schema;

public class Enrollment : BaseEntity
{
    public DateTime EnrolledAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public decimal ProgressPercentage { get; private set; }
    public bool IsActive { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CourseId { get; private set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    // Constructors
    public Enrollment() { }

    // Methods
    public void CreateEnrollment(Guid userId, Guid courseId)
    {
        UserId = userId;
        CourseId = courseId;
        EnrolledAt = DateTime.UtcNow;
        IsActive = true;
        ProgressPercentage = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateProgress(decimal progress)
    {
        ProgressPercentage = progress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CompleteEnrollment()
    {
        CompletedAt = DateTime.UtcNow;
        ProgressPercentage = 100;
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void DeactivateEnrollment()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ReactivateEnrollment()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}