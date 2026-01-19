using AutoMapper;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepo;
    private readonly IMapper _mapper;
    public CourseService(ICourseRepository courseRepo, IMapper mapper)
    {
        _courseRepo = courseRepo;
        _mapper = mapper;
    }

    public async Task<Course> CreateAsync(CreateCourseDto createCourseDto, CancellationToken cancellationToken)
    {
        var course = new Course();
        course.Create(
            createCourseDto.Title,
            createCourseDto.Description,
            createCourseDto.DurationInHours,
            createCourseDto.InstructorId
        );
        await _courseRepo.AddAsync(course, cancellationToken);
        return course;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var course = await _courseRepo.GetByIdAsync(id, cancellationToken);
        if (course == null)
        {
            return false;
        }

        await _courseRepo.DeleteAsync(course, cancellationToken);
        return true;
    }

    public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _courseRepo.GetAllAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _courseRepo.GetByIdAsync(id, cancellationToken);
    }

    public Task<Course?> PublishAsync(Guid id, PublishCourseDto publishCourseDto, CancellationToken cancellationToken)
    {
        return _courseRepo.PublishAsync(id, publishCourseDto.Publish, cancellationToken);
    }

    public async Task<Course?> UpdateAsync(Guid id, UpdateCourseDto updateCourseDto, CancellationToken cancellationToken)
    {
        var existingCourse = await _courseRepo.GetByIdAsync(id, cancellationToken);
        if (existingCourse == null)
        {
            return null;
        }

        existingCourse.UpdateDetails(updateCourseDto.Title, updateCourseDto.Description, updateCourseDto.DurationInHours);

        await _courseRepo.UpdateAsync(existingCourse, cancellationToken);
        return existingCourse;
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(Guid instructorId, CancellationToken cancellationToken)
    {
        return await _courseRepo.GetCoursesByInstructorIdAsync(instructorId, cancellationToken);
    }
}