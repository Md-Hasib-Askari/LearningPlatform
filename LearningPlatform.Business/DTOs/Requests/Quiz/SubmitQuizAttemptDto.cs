public class SubmitQuizAttemptDto
{
    public Guid AttemptId { get; set; }
    public int CorrectAnswers { get; set; }
    public double ScorePercentage { get; set; }
    public bool IsPassed { get; set; }
}
