namespace CleanArchitecture.Domain.Settings;

public sealed class RabbitMqConfiguration
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string ConnectionString => $"amqp://{Username}:{Password}@{Host}:{Port}";
}