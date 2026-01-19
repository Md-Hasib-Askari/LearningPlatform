using LearningPlatform.Data.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Course>> GetPublishedCoursesAsync(CancellationToken cancellationToken = default);
    Task<Course?> PublishAsync(Guid id, bool publish, CancellationToken cancellationToken);
}