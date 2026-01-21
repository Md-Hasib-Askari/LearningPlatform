using Microsoft.EntityFrameworkCore;

public class QuestionOptionRepository : IQuestionOptionRepository
{
    private readonly ApplicationDbContext _db;
    public QuestionOptionRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(QuestionOption entity, CancellationToken cancellationToken = default)
    {
        await _db.QuestionOptions.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<QuestionOption> AddOptionToQuestionAsync(QuestionOption option, CancellationToken cancellationToken = default)
    {
        await _db.QuestionOptions.AddAsync(option, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
        return option;
    }

    public async Task DeleteAsync(QuestionOption entity, CancellationToken cancellationToken = default)
    {
        _db.QuestionOptions.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<QuestionOption>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.QuestionOptions.ToListAsync(cancellationToken);
    }

    public async Task<QuestionOption?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.QuestionOptions.SingleOrDefaultAsync(qo => qo.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<QuestionOption>> GetByIdsAsync(IEnumerable<Guid> optionIds, CancellationToken cancellationToken = default)
    {
        return await _db.QuestionOptions
            .Where(qo => optionIds.Contains(qo.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<QuestionOption>> GetOptionsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default)
    {
        return await _db.QuestionOptions
            .Where(qo => qo.QuestionId == questionId)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(QuestionOption entity, CancellationToken cancellationToken = default)
    {
        _db.QuestionOptions.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}