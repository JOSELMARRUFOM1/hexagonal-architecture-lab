using BudgetService.Domain.Entities;
using BudgetService.Infrastructure.Persistence.Json;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace BudgetService.Infrastructure.Tests.Persistence.Json;

public sealed class JsonBudgetRepositoryTests : IDisposable
{
    private readonly string _directoryPath;
    private readonly string _filePath;

    public JsonBudgetRepositoryTests()
    {
        _directoryPath = Path.Combine(
            Path.GetTempPath(),
            "budget-service-tests",
            Guid.NewGuid().ToString("N"));

        _filePath = Path.Combine(
            _directoryPath,
            "budgets.json");
    }

    [Fact]
    public async Task AddAsync_Should_Persist_Budget()
    {
        // Arrange
        var options = Options.Create(
            new JsonStorageOptions
            {
                FilePath = _filePath
            });

        var writerRepository =
            new JsonBudgetRepository(options);

        var budget = Budget.Create(
            "Internet",
            100m);

        // Act
        await writerRepository.AddAsync(
            budget,
            CancellationToken.None);

        var readerRepository =
            new JsonBudgetRepository(options);

        var budgets = await readerRepository.GetAllAsync(
            CancellationToken.None);

        // Assert
        budgets.Should().ContainSingle();

        var persistedBudget = budgets.Single();

        persistedBudget.Id.Should().Be(budget.Id);
        persistedBudget.Name.Should().Be(budget.Name);
        persistedBudget.Amount.Should().Be(budget.Amount);
        persistedBudget.CreatedAt.Should().Be(budget.CreatedAt);
    }

    [Fact]
    public async Task AddAsync_Should_Preserve_Existing_Budgets()
    {
        // Arrange
        var options = Options.Create(
            new JsonStorageOptions
            {
                FilePath = _filePath
            });

        var repository =
            new JsonBudgetRepository(options);

        var firstBudget = Budget.Create(
            "Internet",
            100m);

        var secondBudget = Budget.Create(
            "Electricity",
            200m);

        // Act
        await repository.AddAsync(
            firstBudget,
            CancellationToken.None);

        await repository.AddAsync(
            secondBudget,
            CancellationToken.None);

        var budgets = await repository.GetAllAsync(
            CancellationToken.None);

        // Assert
        budgets.Should().HaveCount(2);

        budgets
            .Select(budget => budget.Id)
            .Should()
            .BeEquivalentTo(
                 new[]
            {
            firstBudget.Id,
            secondBudget.Id
            });
    }

    public void Dispose()
    {
        if (Directory.Exists(_directoryPath))
        {
            Directory.Delete(
                _directoryPath,
                recursive: true);
        }
    }
}