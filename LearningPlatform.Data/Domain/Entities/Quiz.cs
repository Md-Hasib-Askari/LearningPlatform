using System.ComponentModel.DataAnnotations.Schema;

public class Quiz : BaseEntity
{
    public Guid CourseId { get; private set; }
    public Guid ModuleId { get; private set; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int PassingScore { get; private set; }
    public int TimeLimitMinutes { get; private set; }
    public int MaxAttempts { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation properties
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; } = null!;
    public ICollection<Question> Questions { get; private set; } = new List<Question>();

    // Constructors
    public Quiz() { }

    // Factory Methods
    public static Quiz Create(Guid courseId, Guid moduleId, string title, string description, int passingScore, int timeLimitMinutes, int maxAttempts, bool isActive)
    {
        var quiz = new Quiz();
        quiz.CourseId = courseId;
        quiz.ModuleId = moduleId;
        quiz.Title = title;
        quiz.Description = description;
        quiz.PassingScore = passingScore;
        quiz.TimeLimitMinutes = timeLimitMinutes;
        quiz.MaxAttempts = maxAttempts;
        quiz.IsActive = isActive;
        quiz.CreatedAt = DateTime.UtcNow;
        return quiz;
    }

    public static Quiz Update(Quiz quiz, string title, string description, int passingScore, int timeLimitMinutes, int maxAttempts, bool isActive)
    {
        quiz.Title = title;
        quiz.Description = description;
        quiz.PassingScore = passingScore;
        quiz.TimeLimitMinutes = timeLimitMinutes;
        quiz.MaxAttempts = maxAttempts;
        quiz.IsActive = isActive;
        quiz.UpdatedAt = DateTime.UtcNow;
        return quiz;
    }
}