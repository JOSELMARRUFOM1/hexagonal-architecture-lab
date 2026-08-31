using BudgetService.Domain.Exceptions;
using BudgetService.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace BudgetService.Domain.Tests.Entities;

public sealed class BudgetTests
{
    [Fact]
    public void Should_Create_Budget_When_Data_Is_Valid()
    {
        // Arrange
        const string name = "Internet";
        const decimal amount = 100m;

        // Act
        var budget = Budget.Create(name, amount);

        // Assert
        budget.Should().NotBeNull();
        budget.Id.Should().NotBe(Guid.Empty);
        budget.Name.Should().Be(name);
        budget.Amount.Amount.Should().Be(amount);
        budget.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Should_Throw_DomainException_When_Name_Is_Empty()
    {
        // Arrange
        const string name = "";
        const decimal amount = 100m;

        // Act
        Action act = () => Budget.Create(name, amount);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_Throw_DomainException_When_Amount_Is_Zero()
    {
        // Arrange
        const string name = "Internet";
        const decimal amount = 0m;

        // Act
        Action act = () => Budget.Create(name, amount);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_Throw_DomainException_When_Amount_Is_Negative()
    {
        // Arrange
        const string name = "Internet";
        const decimal amount = -100m;

        // Act
        Action act = () => Budget.Create(name, amount);

        // Assert
        act.Should().Throw<DomainException>();
    }

    [Fact]
    public void Should_Change_Amount_When_New_Amount_Is_Valid()
    {
        // Arrange
        var budget = Budget.Create("Internet", 100m);
        const decimal newAmount = 200m;

        // Act
        budget.ChangeAmount(newAmount);

        // Assert
        budget.Amount.Amount.Should().Be(newAmount);
    }
    [Fact]
    public void Should_Throw_DomainException_When_New_Amount_Is_Invalid()
    {
        // Arrange
        var budget = Budget.Create("Internet", 100m);
        const decimal newAmount = -50m;

        // Act
        Action act = () => budget.ChangeAmount(newAmount);

        // Assert
        act.Should().Throw<DomainException>();
    }
    [Fact]
    public void Should_Rehydrate_Budget_When_Persisted_Data_Is_Valid()
    {
        // Arrange
        var id = Guid.NewGuid();
        const string name = "Internet";
        const decimal amount = 100m;
        var createdAt = new DateTime(
            2026,
            8,
            28,
            12,
            0,
            0,
            DateTimeKind.Utc);

        // Act
        var budget = Budget.Rehydrate(
            id,
            name,
            amount,
            createdAt);

        // Assert
        budget.Id.Should().Be(id);
        budget.Name.Should().Be(name);
        budget.Amount.Amount.Should().Be(amount);
        budget.CreatedAt.Should().Be(createdAt);
    }

}