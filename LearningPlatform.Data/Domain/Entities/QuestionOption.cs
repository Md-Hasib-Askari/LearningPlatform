using System.ComponentModel.DataAnnotations.Schema;

public class QuestionOption : BaseEntity
{
    public Guid QuestionId { get; private set; }
    public string OptionText { get; private set; } = null!;
    public bool IsCorrect { get; private set; }
    public int OrderIndex { get; private set; }

    // Navigation properties
    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; } = null!;

    // Factory Methods
    public static QuestionOption Create(Guid questionId, string optionText, bool isCorrect, int orderIndex)
    {
        var option = new QuestionOption();
        option.QuestionId = questionId;
        option.OptionText = optionText;
        option.IsCorrect = isCorrect;
        option.OrderIndex = orderIndex;
        option.CreatedAt = DateTime.UtcNow;
        return option;
    }

    public static QuestionOption Update(QuestionOption option, string optionText, bool isCorrect, int orderIndex)
    {
        option.OptionText = optionText;
        option.IsCorrect = isCorrect;
        option.OrderIndex = orderIndex;
        option.UpdatedAt = DateTime.UtcNow;
        return option;
    }
}