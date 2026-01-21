public class SubmitQuizRequest
{
    public Guid AttemptId { get; set; }

    // Key: QuestionId, Value: Selected OptionId
    public Dictionary<Guid, Guid> Answers { get; set; } = new();
}