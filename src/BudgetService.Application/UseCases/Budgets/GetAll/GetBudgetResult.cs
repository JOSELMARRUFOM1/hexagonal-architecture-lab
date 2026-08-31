namespace BudgetService.Application.UseCases.Budgets.GetAll;

public sealed record GetBudgetResult(
    Guid Id,
    string Name,
    decimal Amount,
    DateTime CreatedAt);