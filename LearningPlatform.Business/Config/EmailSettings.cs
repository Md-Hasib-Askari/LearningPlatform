public sealed class EmailSettings
{
    public string SmtpHost { get; init; } = default!;
    public int Port { get; init; }
    public string ApiKey { get; init; } = default!;
}
