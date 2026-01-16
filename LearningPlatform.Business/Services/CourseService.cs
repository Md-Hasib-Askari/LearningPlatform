public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepo;
    public CourseService(ICourseRepository courseRepo)
    {
        _courseRepo = courseRepo;
    }

    public async Task<Course> CreateAsync(CreateCourseDto createCourseDto)
    {
        var course = new Course(
            createCourseDto.Title,
            createCourseDto.Description,
            createCourseDto.DurationInHours,
            createCourseDto.InstructorId
        );
        await _courseRepo.AddAsync(course);
        return course;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var course = await _courseRepo.GetByIdAsync(id);
        if (course == null)
        {
            return false;
        }

        await _courseRepo.DeleteAsync(course);
        return true;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _courseRepo.GetAllAsync();
    }

    public async Task<Course?> GetByIdAsync(Guid id)
    {
        return await _courseRepo.GetByIdAsync(id);
    }

    public async Task<Course?> UpdateAsync(Guid id, UpdateCourseDto updateCourseDto)
    {
        var existingCourse = await _courseRepo.GetByIdAsync(id);
        if (existingCourse == null)
        {
            return null;
        }

        existingCourse.UpdateDetails(updateCourseDto.Title, updateCourseDto.Description, updateCourseDto.DurationInHours);

        await _courseRepo.UpdateAsync(existingCourse);
        return existingCourse;
    }
}