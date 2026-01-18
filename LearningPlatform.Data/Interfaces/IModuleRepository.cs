using LearningPlatform.Data.Interfaces;

public interface IModuleRepository : IGenericRepository<Module>
{
    Task<IEnumerable<Module>> GetModulesByCourseIdAsync(Guid courseId, CancellationToken cancellationToken = default);
}