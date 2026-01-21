using System.ComponentModel.DataAnnotations.Schema;

public class QuizAttempt : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid QuizId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public int CorrectAnswers { get; set; }
    public double ScorePercentage { get; set; }
    public bool IsPassed { get; set; }

    // Navigation properties
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    [ForeignKey(nameof(QuizId))]
    public Quiz Quiz { get; set; } = null!;

    // Factory Methods
    public static QuizAttempt Create(Guid userId, Guid quizId)
    {
        var attempt = new QuizAttempt();
        attempt.UserId = userId;
        attempt.QuizId = quizId;
        attempt.StartedAt = DateTime.UtcNow;
        attempt.CorrectAnswers = 0;
        attempt.ScorePercentage = 0;
        attempt.IsPassed = false;
        attempt.CreatedAt = DateTime.UtcNow;
        return attempt;
    }

    public static QuizAttempt Submit(QuizAttempt attempt, int correctAnswers, double scorePercentage, bool isPassed)
    {
        attempt.SubmittedAt = DateTime.UtcNow;
        attempt.CorrectAnswers = correctAnswers;
        attempt.ScorePercentage = scorePercentage;
        attempt.IsPassed = isPassed;
        attempt.UpdatedAt = DateTime.UtcNow;
        return attempt;
    }
}