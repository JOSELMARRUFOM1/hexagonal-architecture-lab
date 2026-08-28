using BudgetService.Domain.ValueObjects;
using BudgetService.Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace BudgetService.Domain.Tests.ValueObjects;

public sealed class MoneyTests
{
    [Fact]
    public void Should_Create_Money_When_Amount_Is_Valid()
    {
        // Arrange
        const decimal amount = 100m;

        // Act
        var money = Money.Create(amount);

        // Assert
        money.Should().NotBeNull();
        money.Amount.Should().Be(amount);
    }

    [Fact]
    public void Should_Throw_DomainException_When_Amount_Is_Zero()
    {
        // Arrange
        const decimal amount = 0m;

        // Act
        Action act = () => Money.Create(amount);

        // Assert
        act.Should().Throw<DomainException>();
    }
    [Fact]
    public void Should_Throw_DomainException_When_Amount_Is_Negative()
    {
        // Arrange
        const decimal amount = -100m;

        // Act
        Action act = () => Money.Create(amount);

        // Assert
        act.Should().Throw<DomainException>();
    }
    [Fact]
    public void Should_Be_Equal_When_Amounts_Are_Equal()
    {
        // Arrange
        const decimal amount = 100m;

        // Act
        var money1 = Money.Create(amount);
        var money2 = Money.Create(amount);

        // Assert
        money1.Should().Be(money2);
    }


}