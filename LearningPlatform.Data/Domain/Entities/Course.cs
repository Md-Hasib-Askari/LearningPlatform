using System.ComponentModel.DataAnnotations.Schema;

public class Course : BaseEntity
{
    public Guid? InstructorId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int DurationInHours { get; private set; }
    public bool IsPublished { get; private set; }

    // Navigation Properties
    [ForeignKey(nameof(InstructorId))]
    public User? Instructor { get; private set; }
    public ICollection<Module> Modules { get; private set; } = new List<Module>();
    public ICollection<Enrollment> Enrollments { get; private set; } = new List<Enrollment>();

    // Constructor
    public Course() { }

    // Factory Methods
    public void Create(string title, string description, int durationInHours, Guid? instructorId)
    {
        Title = title;
        Description = description;
        DurationInHours = durationInHours;
        InstructorId = instructorId ?? Guid.Empty;
        IsPublished = false;
    }

    public void AssignInstructor(Guid instructorId)
    {
        InstructorId = instructorId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Publish(bool publish)
    {
        IsPublished = publish;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string description, int durationInHours)
    {
        Title = title;
        Description = description;
        DurationInHours = durationInHours;
        UpdatedAt = DateTime.UtcNow;
    }
}