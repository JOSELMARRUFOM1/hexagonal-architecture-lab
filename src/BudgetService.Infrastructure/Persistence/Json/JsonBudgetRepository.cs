using System.Text.Json;
using BudgetService.Application.Abstractions.Persistence;
using BudgetService.Domain.Entities;
using Microsoft.Extensions.Options;

namespace BudgetService.Infrastructure.Persistence.Json;

public sealed class JsonBudgetRepository : IBudgetRepository
{
    private static readonly JsonSerializerOptions SerializerOptions =
        new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };

    private readonly string _filePath;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public JsonBudgetRepository(
        IOptions<JsonStorageOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrWhiteSpace(options.Value.FilePath))
        {
            throw new InvalidOperationException(
                "JSON storage file path is required.");
        }

        _filePath = Path.GetFullPath(
            options.Value.FilePath);
    }

    public async Task AddAsync(
        Budget budget,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(budget);

        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            var documents = await ReadDocumentsAsync(
                cancellationToken);

            documents.Add(
                BudgetDocument.FromDomain(budget));

            await WriteDocumentsAsync(
                documents,
                cancellationToken);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<IReadOnlyCollection<Budget>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        await _semaphore.WaitAsync(cancellationToken);

        try
        {
            var documents = await ReadDocumentsAsync(
                cancellationToken);

            return documents
                .Select(document => document.ToDomain())
                .ToArray();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<List<BudgetDocument>> ReadDocumentsAsync(
        CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        await using var stream = new FileStream(
            _filePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true);

        var documents =
            await JsonSerializer.DeserializeAsync<List<BudgetDocument>>(
                stream,
                SerializerOptions,
                cancellationToken);

        return documents ?? [];
    }

    private async Task WriteDocumentsAsync(
        List<BudgetDocument> documents,
        CancellationToken cancellationToken)
    {
        var directoryPath =
            Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        var temporaryFilePath =
            $"{_filePath}.{Guid.NewGuid():N}.tmp";

        try
        {
            await using (var stream = new FileStream(
                temporaryFilePath,
                FileMode.CreateNew,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true))
            {
                await JsonSerializer.SerializeAsync(
                    stream,
                    documents,
                    SerializerOptions,
                    cancellationToken);
            }

            File.Move(
                temporaryFilePath,
                _filePath,
                overwrite: true);
        }
        finally
        {
            if (File.Exists(temporaryFilePath))
            {
                File.Delete(temporaryFilePath);
            }
        }
    }
}