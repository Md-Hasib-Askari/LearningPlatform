public sealed class EmailSettings
{
    public string SmtpHost { get; init; } = default!;
    public int Port { get; init; }
    public string FromAddress { get; init; } = default!;
    public string ApiKey { get; init; } = default!;
}
