using BudgetService.Application.Abstractions.Persistence;
using BudgetService.Domain.Entities;

namespace BudgetService.Application.UseCases.Budgets.Create;

public sealed class CreateBudgetUseCase
{
    private readonly IBudgetRepository _budgetRepository;

    public CreateBudgetUseCase(
        IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<CreateBudgetResult> ExecuteAsync(
        CreateBudgetCommand command,
        CancellationToken cancellationToken)
    {
        var budget = Budget.Create(
            command.Name,
            command.Amount);

        await _budgetRepository.AddAsync(
            budget,
            cancellationToken);


        return new CreateBudgetResult(
            budget.Id,
            budget.Name,
            budget.Amount.Amount,
            budget.CreatedAt);
    }
}