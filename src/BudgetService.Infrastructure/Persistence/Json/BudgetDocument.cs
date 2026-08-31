using BudgetService.Domain.Entities;

namespace BudgetService.Infrastructure.Persistence.Json;

internal sealed record BudgetDocument(
    Guid Id,
    string Name,
    decimal Amount,
    DateTime CreatedAt)
{
    public static BudgetDocument FromDomain(
        Budget budget)
    {
        return new BudgetDocument(
            budget.Id,
            budget.Name,
            budget.Amount.Amount,
            budget.CreatedAt);
    }

    public Budget ToDomain()
    {
        return Budget.Rehydrate(
            Id,
            Name,
            Amount,
            CreatedAt);
    }
}