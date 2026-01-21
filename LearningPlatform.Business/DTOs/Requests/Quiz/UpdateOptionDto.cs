public class UpdateOptionDto
{
    public Guid OptionId { get; set; }
    public string OptionText { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public int OrderIndex { get; set; }
}
