using LearningPlatform.Data.Interfaces;

public interface ICourseRepository : IGenericRepository<Course>
{
    Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(Guid instructorId);
    Task<IEnumerable<Course>> GetPublishedCoursesAsync();
}