using LearningPlatform.Data.Interfaces;

public interface ILessonRepository : IGenericRepository<Lesson>
{
    Task<IEnumerable<Lesson>> GetLessonsByModuleIdAsync(Guid moduleId, CancellationToken cancellationToken = default);
}