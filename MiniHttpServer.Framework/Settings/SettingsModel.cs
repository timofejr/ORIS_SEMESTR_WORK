namespace MiniHttpServer.Framework.Settings;

public class SettingsModel
{
    public string StaticFilesPath { get; init; }
    public string Domain { get; init; }
    public string Port { get; init; }
    public string EmailAddressFrom { get; init; }
    public string EmailNameFrom { get; init; }
    public int SmtpPort { get; init; }
    public string SmtpHost { get; init; }
    public string SmtpPassword { get; init; }
    public string ConnectionString { get; init; } = Environment.GetEnvironmentVariable("ConnectionString");
    // public string ConnectionString { get; init; } = "Host=localhost;Port=5432;Database=oris_db;Username=oris_user;Password=11408";
}