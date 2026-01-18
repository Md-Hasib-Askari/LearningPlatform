using System.ComponentModel.DataAnnotations.Schema;

public class Lesson : BaseEntity
{
    public string Title { get; private set; } = null!;
    public string ContentType { get; private set; } = null!;
    public string ContentUrl { get; private set; } = null!;
    public int OrderIndex { get; private set; }
    public bool IsCompleted { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    // Navigation Properties
    public Guid ModuleId { get; private set; }

    [ForeignKey(nameof(ModuleId))]
    public Module Module { get; set; } = null!;


    // Factory Methods
    public Lesson() { }

    public void Create(string title, string contentType, string contentUrl, int orderIndex, Guid moduleId)
    {
        Title = title;
        ContentType = contentType;
        ContentUrl = contentUrl;
        OrderIndex = orderIndex;
        ModuleId = moduleId;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string title, string contentType, string contentUrl)
    {
        Title = title;
        ContentType = contentType;
        ContentUrl = contentUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetModule(Guid moduleId)
    {
        ModuleId = moduleId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsComplete()
    {
        IsCompleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}