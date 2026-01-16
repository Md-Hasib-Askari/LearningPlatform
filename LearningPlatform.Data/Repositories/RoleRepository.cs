using Microsoft.EntityFrameworkCore;

public class RoleRepository : IGenericRepository<Role>, IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Role entity, CancellationToken cancellationToken = default)
    {
        await _context.Roles.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Role entity, CancellationToken cancellationToken = default)
    {
        _context.Roles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Roles.ToListAsync<Role>(cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Role>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
            .ThenInclude(ur => ur.User)
            .Where(r => r.UserRoles.Any(ur => ur.UserId == userId))
            .ToListAsync<Role>(cancellationToken);
    }

    public async Task UpdateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        _context.Roles.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}