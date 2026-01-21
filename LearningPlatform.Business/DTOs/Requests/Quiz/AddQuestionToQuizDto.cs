public class AddQuestionToQuizDto
{
    public Guid QuizId { get; set; }
    public string QuestionText { get; set; } = null!;
    public string QuestionType { get; set; } = null!;
    public int Points { get; set; }
    public int OrderIndex { get; set; }
}