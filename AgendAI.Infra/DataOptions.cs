using Microsoft.Extensions.Configuration;

namespace AgendAI.Infra;

public static class DataOptions
{
    public const string SectionName = "Data";
    public const string UseInMemoryKey = "UseInMemory";
    public const string InMemoryDatabaseName = "AgendAi";
    public const string UseInMemoryEnvironmentVariable = "USE_IN_MEMORY_DATABASE";

    public static bool UseInMemory(IConfiguration configuration)
    {
        if (configuration.GetValue<bool?>($"{SectionName}:{UseInMemoryKey}") == true)
            return true;

        var envValue = Environment.GetEnvironmentVariable(UseInMemoryEnvironmentVariable);
        return string.Equals(envValue, "true", StringComparison.OrdinalIgnoreCase)
            || string.Equals(envValue, "1", StringComparison.OrdinalIgnoreCase);
    }
}
