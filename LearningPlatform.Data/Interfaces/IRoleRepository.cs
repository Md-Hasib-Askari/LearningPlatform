public interface IRoleRepository : IGenericRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default);
}