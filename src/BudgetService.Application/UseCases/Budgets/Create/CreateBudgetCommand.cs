namespace BudgetService.Application.UseCases.Budgets.Create;

public sealed record CreateBudgetCommand(
    string Name,
    decimal Amount);