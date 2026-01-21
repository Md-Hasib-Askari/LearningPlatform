using System.ComponentModel.DataAnnotations.Schema;

public class UserProgress : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid LessonId { get; private set; }
    public DateTime LastAccessedAt { get; private set; }
    public bool IsCompleted { get; private set; }
    public int TimeSpentSeconds { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(LessonId))]
    public Lesson Lesson { get; set; } = null!;

    // Constructors
    public UserProgress() { }

    // Methods
    public void CreateProgress(Guid userId, Guid lessonId)
    {
        UserId = userId;
        LessonId = lessonId;
        LastAccessedAt = DateTime.UtcNow;
        TimeSpentSeconds = 0;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }
    public void UpdateProgress(bool isCompleted, int timeSpentSeconds = 0)
    {
        IsCompleted = isCompleted;
        if (isCompleted)
        {
            CompletedAt = DateTime.UtcNow;
        }
        LastAccessedAt = DateTime.UtcNow;
        TimeSpentSeconds = timeSpentSeconds;
        UpdatedAt = DateTime.UtcNow;
    }
    public void UpdateLastAccessed(DateTime lastAccessed, int timeSpentSeconds = 0)
    {
        TimeSpentSeconds = timeSpentSeconds;
        LastAccessedAt = lastAccessed;
        UpdatedAt = DateTime.UtcNow;
    }
}