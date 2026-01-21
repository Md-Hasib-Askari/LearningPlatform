using Microsoft.EntityFrameworkCore;

public class QuizAttemptRepository : IQuizAttemptRepository
{
    private readonly ApplicationDbContext _db;

    public QuizAttemptRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(QuizAttempt entity, CancellationToken cancellationToken = default)
    {
        await _db.QuizAttempts.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(QuizAttempt entity, CancellationToken cancellationToken = default)
    {
        _db.QuizAttempts.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<QuizAttempt>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.QuizAttempts.ToListAsync(cancellationToken);
    }

    public async Task<QuizAttempt?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.QuizAttempts.SingleOrDefaultAsync(qa => qa.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<QuizAttempt>> GetQuizAttemptsByUserAsync(Guid userId)
    {
        return await _db.QuizAttempts
            .Where(qa => qa.UserId == userId)
            .ToListAsync();
    }

    public async Task<QuizAttempt> StartQuizAttemptAsync(QuizAttempt attempt)
    {
        await _db.QuizAttempts.AddAsync(attempt);
        await _db.SaveChangesAsync();
        return attempt;
    }

    public async Task<QuizAttempt> SubmitQuizAttemptAsync(QuizAttempt attempt)
    {
        _db.QuizAttempts.Update(attempt);
        await _db.SaveChangesAsync();
        return attempt;
    }

    public async Task UpdateAsync(QuizAttempt entity, CancellationToken cancellationToken = default)
    {
        _db.QuizAttempts.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}