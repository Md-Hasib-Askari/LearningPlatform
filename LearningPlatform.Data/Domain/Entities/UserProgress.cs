using System.ComponentModel.DataAnnotations.Schema;

public class UserProgress : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid LessonId { get; private set; }
    public DateTime LastAccessedAt { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }

    [ForeignKey(nameof(LessonId))]
    public required Lesson Lesson { get; set; }
    // Constructors
    public UserProgress() { }

    // Methods
    public void CreateProgress(Guid userId, Guid lessonId)
    {
        UserId = userId;
        LessonId = lessonId;
        LastAccessedAt = DateTime.UtcNow;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }
    public void UpdateProgress(bool isCompleted)
    {
        IsCompleted = isCompleted;
        if (isCompleted)
        {
            CompletedAt = DateTime.UtcNow;
        }
        LastAccessedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateLastAccessed(DateTime lastAccessed)
    {
        LastAccessedAt = lastAccessed;
        UpdatedAt = DateTime.UtcNow;
    }
}