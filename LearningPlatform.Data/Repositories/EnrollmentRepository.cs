using Microsoft.EntityFrameworkCore;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly ApplicationDbContext _db;
    public EnrollmentRepository(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }

    public async Task AddAsync(Enrollment entity, CancellationToken cancellationToken = default)
    {
        await _db.Enrollments.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Enrollment entity, CancellationToken cancellationToken = default)
    {
        _db.Enrollments.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Enrollments.ToListAsync(cancellationToken);
    }

    public async Task<Enrollment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Enrollments.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Enrollment?> GetByUserIdAndCourseIdAsync(Guid userId, Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _db.Enrollments
            .SingleOrDefaultAsync(e => e.UserId == userId && e.CourseId == courseId, cancellationToken);
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _db.Enrollments
            .Where(e => e.CourseId == courseId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _db.Enrollments
            .Where(e => e.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetStudentsEnrolledInCourseAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _db.Enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.User)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Enrollment entity, CancellationToken cancellationToken = default)
    {
        _db.Enrollments.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}