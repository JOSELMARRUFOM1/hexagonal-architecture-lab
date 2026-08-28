namespace BudgetService.Application.UseCases.Budgets.Create;

public sealed record CreateBudgetResult(
    Guid Id,
    string Name,
    decimal Amount,
    DateTime CreatedAt);