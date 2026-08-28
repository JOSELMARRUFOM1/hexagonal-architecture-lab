using BudgetService.Domain.Entities;

namespace BudgetService.Application.Abstractions.Persistence;

public interface IBudgetRepository
{
    Task AddAsync(
        Budget budget,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<Budget>> GetAllAsync(
        CancellationToken cancellationToken);
}