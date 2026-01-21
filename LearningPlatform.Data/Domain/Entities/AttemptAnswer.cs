public class AttemptAnswer : BaseEntity
{
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid SelectedOptionId { get; set; }
    public bool IsCorrect { get; set; }
    public int ScoreAwarded { get; set; }

    // Navigation properties
    public QuizAttempt Attempt { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public QuestionOption SelectedOption { get; set; } = null!;

    // Constructors
    public AttemptAnswer() { }

    // Factory methods
    public static AttemptAnswer Create(Guid attemptId, Guid questionId, Guid selectedOptionId, bool isCorrect, int scoreAwarded)
    {
        return new AttemptAnswer
        {
            AttemptId = attemptId,
            QuestionId = questionId,
            SelectedOptionId = selectedOptionId,
            IsCorrect = isCorrect,
            ScoreAwarded = scoreAwarded
        };
    }

    // Methods
    public void UpdateAnswer(Guid selectedOptionId, bool isCorrect, int scoreAwarded)
    {
        SelectedOptionId = selectedOptionId;
        IsCorrect = isCorrect;
        ScoreAwarded = scoreAwarded;
    }
}