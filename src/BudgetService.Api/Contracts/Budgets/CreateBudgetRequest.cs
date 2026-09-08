namespace BudgetService.Api.Contracts.Budgets;

public sealed record CreateBudgetRequest(
    string Name,
    decimal Amount);