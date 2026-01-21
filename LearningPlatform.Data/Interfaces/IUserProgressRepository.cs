using LearningPlatform.Data.Interfaces;

public interface IUserProgressRepository : IGenericRepository<UserProgress>
{
    Task<UserProgress?> GetByUserAndLessonAsync(Guid userId, Guid lessonId);
    Task<IEnumerable<UserProgress>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserProgress>> GetCompletedLessonsByUserIdAsync(Guid userId);
    Task<int> GetCompletedLessonCountByUserIdAsync(Guid userId);
    Task<bool> HasUserCompletedLessonAsync(Guid userId, Guid lessonId);
    Task<IEnumerable<UserProgress>> GetByCourseIdAsync(Guid courseId, Guid userId);
    Task UpdateUserProgressAsync(Guid userId, Guid lessonId, bool isCompleted);
}