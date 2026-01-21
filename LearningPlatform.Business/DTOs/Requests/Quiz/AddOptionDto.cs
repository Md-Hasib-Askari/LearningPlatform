public class AddOptionDto
{
    public Guid QuestionId { get; set; }
    public string OptionText { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}