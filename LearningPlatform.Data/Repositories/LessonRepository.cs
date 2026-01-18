using Microsoft.EntityFrameworkCore;

public class LessonRepository : ILessonRepository
{
    private readonly ApplicationDbContext _db;
    public LessonRepository(ApplicationDbContext context)
    {
        _db = context;
    }

    public async Task AddAsync(Lesson entity, CancellationToken cancellationToken = default)
    {
        await _db.Lessons.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Lesson entity, CancellationToken cancellationToken = default)
    {
        _db.Lessons.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Lesson>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Lessons.ToListAsync(cancellationToken);
    }

    public async Task<Lesson?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Lessons.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Lesson>> GetLessonsByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default)
    {
        return await _db.Lessons.Where(l => l.ModuleId == moduleId).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Lesson entity, CancellationToken cancellationToken = default)
    {
        _db.Lessons.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}