public class LessonDto
{

    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public Guid ModuleId { get; set; }
}