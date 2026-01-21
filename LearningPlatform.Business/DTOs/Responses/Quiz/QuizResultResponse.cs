public class QuizResultResponse
{
    public Guid AttemptId { get; set; }
    public Guid QuizId { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public double ScorePercentage { get; set; }
    public bool IsPassed { get; set; }
}