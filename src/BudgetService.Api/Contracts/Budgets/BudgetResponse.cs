namespace BudgetService.Api.Contracts.Budgets;

public sealed record BudgetResponse(
    Guid Id,
    string Name,
    decimal Amount,
    DateTime CreatedAt);