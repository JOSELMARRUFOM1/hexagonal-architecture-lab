using BudgetService.Application.Abstractions.Persistence;

namespace BudgetService.Application.UseCases.Budgets.GetAll;

public sealed class GetAllBudgetsUseCase
{
    private readonly IBudgetRepository _budgetRepository;

    public GetAllBudgetsUseCase(
        IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<IReadOnlyCollection<GetBudgetResult>> ExecuteAsync(
        CancellationToken cancellationToken)
    {
        var budgets = await _budgetRepository.GetAllAsync(
            cancellationToken);

        return budgets
            .Select(
                budget => new GetBudgetResult(
                    budget.Id,
                    budget.Name,
                    budget.Amount.Amount,
                    budget.CreatedAt))
            .ToArray();
    }
}