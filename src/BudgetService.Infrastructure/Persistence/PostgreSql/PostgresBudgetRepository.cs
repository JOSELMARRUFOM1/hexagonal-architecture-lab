using BudgetService.Application.Abstractions.Persistence;
using BudgetService.Domain.Entities;
using Npgsql;

namespace BudgetService.Infrastructure.Persistence.PostgreSql;

public sealed class PostgresBudgetRepository : IBudgetRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PostgresBudgetRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task AddAsync(
        Budget budget,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO public.budgets (id, name, amount, created_at)
            VALUES ($1, $2, $3, $4);
            """;

        await using var command = _dataSource.CreateCommand(sql);

        command.Parameters.Add(
            new NpgsqlParameter { Value = budget.Id });
        command.Parameters.Add(
            new NpgsqlParameter { Value = budget.Name });
        command.Parameters.Add(
            new NpgsqlParameter { Value = budget.Amount.Amount });
        command.Parameters.Add(
            new NpgsqlParameter { Value = budget.CreatedAt });

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Budget>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT id, name, amount, created_at
            FROM public.budgets
            ORDER BY created_at, id;
            """;

        var budgets = new List<Budget>();

        await using var command = _dataSource.CreateCommand(sql);
        await using var reader =
            await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var budget = Budget.Rehydrate(
                reader.GetGuid(0),
                reader.GetString(1),
                reader.GetDecimal(2),
                reader.GetDateTime(3));

            budgets.Add(budget);
        }

        return budgets;
    }
}
