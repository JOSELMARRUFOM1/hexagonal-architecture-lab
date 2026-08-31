using BudgetService.Application.Abstractions.Persistence;
using BudgetService.Application.UseCases.Budgets.GetAll;
using BudgetService.Domain.Entities;
using FluentAssertions;
using Moq;

namespace BudgetService.Application.Tests.UseCases.Budgets.GetAll;

public sealed class GetAllBudgetsUseCaseTests
{
    [Fact]
    public async Task Should_Return_All_Budgets()
    {
        // Arrange
        var firstBudget = Budget.Create(
            "Internet",
            100m);

        var secondBudget = Budget.Create(
            "Electricity",
            200m);

        IReadOnlyCollection<Budget> budgets =
            new[]
            {
                firstBudget,
                secondBudget
            };

        var repositoryMock =
            new Mock<IBudgetRepository>();

        repositoryMock
            .Setup(repository => repository.GetAllAsync(
                CancellationToken.None))
            .ReturnsAsync(budgets);

        var useCase = new GetAllBudgetsUseCase(
            repositoryMock.Object);

        // Act
        var result = await useCase.ExecuteAsync(
            CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);

        result
            .Select(budget => budget.Id)
            .Should()
            .BeEquivalentTo(
                new[]
                {
                    firstBudget.Id,
                    secondBudget.Id
                });
    }
}