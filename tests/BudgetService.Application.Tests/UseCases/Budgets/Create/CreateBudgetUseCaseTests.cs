using BudgetService.Application.Abstractions.Persistence;
using BudgetService.Application.UseCases.Budgets.Create;
using BudgetService.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace BudgetService.Application.Tests.UseCases.Budgets.Create;

public sealed class CreateBudgetUseCaseTests
{
    [Fact]
    public async Task Should_Create_Budget_When_Command_Is_Valid()
    {
        // Arrange
        var repositoryMock = new Mock<IBudgetRepository>();

        var useCase = new CreateBudgetUseCase(
            repositoryMock.Object);

        var command = new CreateBudgetCommand(
            "Internet",
            100m);

        // Act
        var result = await useCase.ExecuteAsync(
            command,
            CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Name.Should().Be("Internet");
        result.Amount.Should().Be(100m);
    }
}