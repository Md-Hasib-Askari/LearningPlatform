public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllAsync();
    Task<Course?> GetByIdAsync(Guid id);
    Task<Course> CreateAsync(CreateCourseDto createCourseDto);
    Task<Course?> UpdateAsync(Guid id, UpdateCourseDto updateCourseDto);
    Task<bool> DeleteAsync(Guid id);
}