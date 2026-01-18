using Microsoft.EntityFrameworkCore;

public class ModuleRepository : IModuleRepository
{
    private readonly ApplicationDbContext _db;

    public ModuleRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Module entity, CancellationToken cancellationToken = default)
    {
        await _db.Modules.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Module entity, CancellationToken cancellationToken = default)
    {
        _db.Modules.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Module>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var modules = await _db.Modules.ToListAsync<Module>(cancellationToken);
        return modules;
    }

    public async Task<Module?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Modules.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default)
    {
        return await _db.Modules.Where(m => m.CourseId == courseId).ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Module entity, CancellationToken cancellationToken = default)
    {
        _db.Modules.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}