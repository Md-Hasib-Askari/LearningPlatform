public class CreateLessonDto
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid ModuleId { get; set; }
}