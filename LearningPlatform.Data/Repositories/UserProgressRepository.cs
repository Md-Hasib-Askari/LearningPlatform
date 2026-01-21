using Microsoft.EntityFrameworkCore;

public class UserProgressRepository : IUserProgressRepository
{
    private readonly ApplicationDbContext _db;
    public UserProgressRepository(ApplicationDbContext context)
    {
        _db = context;
    }

    public async Task AddAsync(UserProgress entity, CancellationToken cancellationToken = default)
    {
        await _db.UserProgresses.AddAsync(entity, cancellationToken);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(UserProgress entity, CancellationToken cancellationToken = default)
    {
        _db.UserProgresses.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<UserProgress>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.UserProgresses.ToListAsync(cancellationToken);
    }

    public Task<IEnumerable<UserProgress>> GetByCourseIdAsync(Guid courseId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<UserProgress?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.UserProgresses.SingleOrDefaultAsync(up => up.Id == id, cancellationToken);
    }

    public async Task<UserProgress?> GetByUserAndLessonAsync(Guid userId, Guid lessonId)
    {
        return await _db.UserProgresses
            .FirstOrDefaultAsync(up => up.UserId == userId && up.LessonId == lessonId);
    }

    public async Task<IEnumerable<UserProgress>> GetByUserIdAsync(Guid userId)
    {
        return await _db.UserProgresses
            .Where(up => up.UserId == userId)
            .ToListAsync();
    }

    public async Task<int> GetCompletedLessonCountByUserIdAsync(Guid userId)
    {
        return await _db.UserProgresses
            .CountAsync(up => up.UserId == userId && up.IsCompleted);
    }

    public async Task<IEnumerable<UserProgress>> GetCompletedLessonsByUserIdAsync(Guid userId)
    {
        return await _db.UserProgresses
            .Where(up => up.UserId == userId && up.IsCompleted)
            .ToListAsync();
    }

    public async Task<bool> HasUserCompletedLessonAsync(Guid userId, Guid lessonId)
    {
        return await _db.UserProgresses
            .AnyAsync(up => up.UserId == userId && up.LessonId == lessonId && up.IsCompleted);
    }

    public async Task UpdateAsync(UserProgress entity, CancellationToken cancellationToken = default)
    {
        _db.UserProgresses.Update(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateUserProgressAsync(Guid userId, Guid lessonId, bool isCompleted)
    {
        var userProgress = await _db.UserProgresses
            .FirstOrDefaultAsync(up => up.UserId == userId && up.LessonId == lessonId);

        if (userProgress != null)
        {
            userProgress.UpdateProgress(isCompleted);
            _db.UserProgresses.Update(userProgress);
            await _db.SaveChangesAsync();
        }
        else
        {
            var newUserProgress = new UserProgress();
            newUserProgress.CreateProgress(userId, lessonId);
            if (isCompleted)
            {
                newUserProgress.UpdateProgress(isCompleted);
            }
            await _db.UserProgresses.AddAsync(newUserProgress);
            await _db.SaveChangesAsync();
        }
    }
}