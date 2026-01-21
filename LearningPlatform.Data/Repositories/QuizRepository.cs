using LearningPlatform.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

public class QuizRepository : IQuizRepository
{
    private readonly ApplicationDbContext _db;

    public QuizRepository(ApplicationDbContext context)
    {
        _db = context;
    }

    public async Task AddAsync(Quiz entity, CancellationToken cancellationToken = default)
    {
        await _db.Quizzes.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Quiz entity, CancellationToken cancellationToken = default)
    {
        _db.Quizzes.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Quiz>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Quizzes.ToListAsync(cancellationToken);
    }

    public async Task<Quiz?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Quizzes.SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Quiz>> GetQuizzesByCourseAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _db.Quizzes
            .Where(q => q.CourseId == courseId)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Quiz entity, CancellationToken cancellationToken = default)
    {
        _db.Quizzes.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}