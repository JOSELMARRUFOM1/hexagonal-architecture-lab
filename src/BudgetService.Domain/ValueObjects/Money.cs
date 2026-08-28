using BudgetService.Domain.Exceptions;

namespace BudgetService.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    private Money(decimal amount)
    {
        Amount = amount;
    }

    public static Money Create(decimal amount)
    {
        if (amount <= 0)
        {
            throw new DomainException(
                "Amount must be greater than zero.");
        }

        return new Money(amount);
    }
}