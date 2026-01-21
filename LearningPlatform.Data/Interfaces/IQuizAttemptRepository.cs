using LearningPlatform.Data.Interfaces;

public interface IQuizAttemptRepository : IGenericRepository<QuizAttempt>
{
    Task<QuizAttempt> StartQuizAttemptAsync(QuizAttempt attempt);
    Task<QuizAttempt> SubmitQuizAttemptAsync(QuizAttempt attempt);
    Task<IEnumerable<QuizAttempt>> GetQuizAttemptsByUserAsync(Guid userId);

}