namespace FieldUp.Infrastructure.Configurations;

public class PostgresOptions
{
    public const string SectionKey = "Postgres";
    public const string PasswordSecretKey = "MartenDbPassword";
    
    public string Host { get; init; } = string.Empty;
    public string Port { get; init; } = string.Empty;
    public string Database { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    
    public string BuildConnectionString(string password)
    {
        return $"host={Host};port={Port};database={Database};username={Username};password={password};";
    }
}