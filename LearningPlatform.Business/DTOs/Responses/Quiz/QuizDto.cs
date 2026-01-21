public class QuizDto
{
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid ModuleId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int PassingScore { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int MaxAttempts { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<QuestionDto> Questions { get; set; } = new();
}
