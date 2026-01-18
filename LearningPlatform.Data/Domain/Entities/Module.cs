using System.ComponentModel.DataAnnotations.Schema;

public class Module : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int OrderIndex { get; private set; }
    public Guid CourseId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation Properties
    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; private set; } = new List<Lesson>();

    // Factory Methods
    public Module() { }

    public void Create(string title, string description, int orderIndex, Guid courseId)
    {
        Title = title;
        Description = description;
        OrderIndex = orderIndex;
        CourseId = courseId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description)
    {
        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetCourse(Guid courseId)
    {
        CourseId = courseId;
        UpdatedAt = DateTime.UtcNow;
    }
}
