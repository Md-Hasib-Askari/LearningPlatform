using LearningPlatform.Data.Interfaces;

public interface IQuestionRepository : IGenericRepository<Question>
{
    Task<Question> AddQuestionToQuizAsync(Question question, CancellationToken cancellationToken = default);
}