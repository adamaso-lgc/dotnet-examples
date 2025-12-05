namespace FieldUp.Infrastructure.Configurations;

public class EmailOptions
{
    public const string SectionKey = "Email";
    public string From { get; set; } = string.Empty;
    public SmtpSettings Smtp { get; set; } = new();
}

public class SmtpSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string SmtpPasswordSecretKey { get; set; } = string.Empty;
}