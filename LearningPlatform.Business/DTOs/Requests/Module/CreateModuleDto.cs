public class CreateModuleDto
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public Guid CourseId { get; set; }
}