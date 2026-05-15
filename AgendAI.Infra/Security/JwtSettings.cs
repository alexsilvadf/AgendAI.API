namespace AgendAI.Infra.Security;

public sealed class JwtSettings
{
    public const string SectionName = "Jwt";

    public string Secret { get; set; } = string.Empty;

    public string Issuer { get; set; } = "AgendAI";

    public string Audience { get; set; } = "AgendAI.App";

    public int ExpirationMinutes { get; set; } = 480;
}
