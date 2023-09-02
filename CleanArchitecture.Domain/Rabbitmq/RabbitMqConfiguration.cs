namespace CleanArchitecture.Domain.Rabbitmq;

public sealed class RabbitMqConfiguration
{
    public string Host { get; set; } = string.Empty;
    public bool Enabled { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}