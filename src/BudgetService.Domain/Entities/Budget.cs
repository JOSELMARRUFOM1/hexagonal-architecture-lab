using BudgetService.Domain.Exceptions;
using BudgetService.Domain.ValueObjects;

namespace BudgetService.Domain.Entities;

public sealed class Budget
{
    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public Money Amount { get; private set; } = null!;

    public DateTime CreatedAt { get; private set; }

    private Budget()
    {
    }

    public static Budget Create(string name, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Budget name is required.");
        }

        return new Budget
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Amount = Money.Create(amount),
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Budget Rehydrate(
    Guid id,
    string name,
    decimal amount,
    DateTime createdAt)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("Budget id is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Budget name is required.");
        }

        if (createdAt == default)
        {
            throw new DomainException(
                "Budget creation date is required.");
        }

        return new Budget
        {
            Id = id,
            Name = name.Trim(),
            Amount = Money.Create(amount),
            CreatedAt = createdAt
        };
    }

    public void ChangeAmount(decimal newAmount)
    {
        Amount = Money.Create(newAmount);
    }
}