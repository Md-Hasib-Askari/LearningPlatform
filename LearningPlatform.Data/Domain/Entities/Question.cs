using System.ComponentModel.DataAnnotations.Schema;

public class Question : BaseEntity
{
    public Guid QuizId { get; private set; }
    public string QuestionText { get; private set; } = null!;
    public string QuestionType { get; private set; } = null!;
    public int Points { get; private set; }
    public int OrderIndex { get; private set; }

    // Navigation properties
    [ForeignKey(nameof(QuizId))]
    public Quiz Quiz { get; set; } = null!;
    public ICollection<QuestionOption> Options { get; private set; } = new List<QuestionOption>();

    // Factory Methods
    public static Question Create(Guid quizId, string questionText, string questionType, int points, int orderIndex)
    {
        var question = new Question();
        question.QuizId = quizId;
        question.QuestionText = questionText;
        question.QuestionType = questionType;
        question.Points = points;
        question.OrderIndex = orderIndex;
        question.CreatedAt = DateTime.UtcNow;
        return question;
    }

    public static Question Update(Question question, string questionText, string questionType, int points, int orderIndex)
    {
        question.QuestionText = questionText;
        question.QuestionType = questionType;
        question.Points = points;
        question.OrderIndex = orderIndex;
        question.UpdatedAt = DateTime.UtcNow;
        return question;
    }
}