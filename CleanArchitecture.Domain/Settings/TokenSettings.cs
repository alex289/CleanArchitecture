namespace CleanArchitecture.Domain.Settings;

public sealed class TokenSettings
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Secret { get; set; } = null!;
}