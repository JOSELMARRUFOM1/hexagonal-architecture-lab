namespace BudgetService.Infrastructure.Persistence.Json;

public sealed class JsonStorageOptions
{
    public const string SectionName = "JsonStorage";

    public string FilePath { get; set; } = string.Empty;
}