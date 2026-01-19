public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken);
    Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Course> CreateAsync(CreateCourseDto createCourseDto, CancellationToken cancellationToken);
    Task<Course?> UpdateAsync(Guid id, UpdateCourseDto updateCourseDto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task<Course?> PublishAsync(Guid id, PublishCourseDto publishCourseDto, CancellationToken cancellationToken);
    Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken);
}