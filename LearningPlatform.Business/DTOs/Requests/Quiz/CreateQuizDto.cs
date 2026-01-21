public class CreateQuizDto
{
    public Guid CourseId { get; set; }
    public Guid ModuleId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int PassingScore { get; set; }
    public int TimeLimitMinutes { get; set; }
    public int MaxAttempts { get; set; }
    public bool IsActive { get; set; }
}