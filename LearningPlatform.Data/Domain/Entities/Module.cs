using System.ComponentModel.DataAnnotations.Schema;

public class Module : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public int OrderIndex { get; private set; }
    public Guid CourseId { get; private set; }

    [ForeignKey(nameof(CourseId))]
    public Course Course { get; set; } = null!;

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Module(string title, string description, Guid courseId)
    {
        Title = title;
        Description = description;
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

// Id (PK)         │       │ Progress %       │
// │ CourseId (FK)   │       └──────────────────┘
// │ Title           │
// │ Description     │              │
// │ OrderIndex      │         1:N  │
// │ CreatedAt