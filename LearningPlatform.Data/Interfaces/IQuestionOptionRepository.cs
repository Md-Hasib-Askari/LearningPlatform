using LearningPlatform.Data.Interfaces;

public interface IQuestionOptionRepository : IGenericRepository<QuestionOption>
{
    Task<QuestionOption> AddOptionToQuestionAsync(QuestionOption option, CancellationToken cancellationToken = default);
    Task<IEnumerable<QuestionOption>> GetOptionsByQuestionIdAsync(Guid questionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<QuestionOption>> GetByIdsAsync(IEnumerable<Guid> optionIds, CancellationToken cancellationToken = default);
}