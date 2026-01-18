public class CreateLessonDto
{
    public string Title { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public string ContentUrl { get; set; } = null!;
    public int OrderIndex { get; set; }
    public Guid ModuleId { get; set; }
}