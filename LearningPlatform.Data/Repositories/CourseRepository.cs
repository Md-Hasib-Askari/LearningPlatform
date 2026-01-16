using LearningPlatform.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

public class CourseRepository : IGenericRepository<Course>, ICourseRepository
{
    private readonly ApplicationDbContext _db;
    public CourseRepository(ApplicationDbContext context) { _db = context; }

    public async Task AddAsync(Course entity, CancellationToken cancellationToken = default)
    {
        await _db.Courses.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Course entity, CancellationToken cancellationToken = default)
    {
        _db.Courses.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Courses.ToListAsync(cancellationToken);
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Courses.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorIdAsync(Guid instructorId)
    {
        return await _db.Courses
            .Where(c => c.InstructorId == instructorId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetPublishedCoursesAsync()
    {
        return await _db.Courses
            .Where(c => c.IsPublished)
            .ToListAsync();
    }

    public async Task UpdateAsync(Course entity, CancellationToken cancellationToken = default)
    {
        _db.Courses.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}