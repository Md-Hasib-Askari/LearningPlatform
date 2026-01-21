using Microsoft.EntityFrameworkCore;

public class QuestionRepository : IQuestionRepository
{
    private readonly ApplicationDbContext _db;

    public QuestionRepository(ApplicationDbContext context)
    {
        _db = context;
    }

    public async Task AddAsync(Question entity, CancellationToken cancellationToken = default)
    {
        await _db.Questions.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Question> AddQuestionToQuizAsync(Question question, CancellationToken cancellationToken = default)
    {
        await _db.Questions.AddAsync(question, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return question;
    }

    public async Task DeleteAsync(Question entity, CancellationToken cancellationToken = default)
    {
        _db.Questions.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Question>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Questions.ToListAsync(cancellationToken);
    }

    public async Task<Question?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Questions.SingleOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task UpdateAsync(Question entity, CancellationToken cancellationToken = default)
    {
        _db.Questions.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}