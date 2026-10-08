using BudgetService.Application.Abstractions.Persistence;
using BudgetService.Application.UseCases.Budgets.Create;
using BudgetService.Application.UseCases.Budgets.GetAll;
using BudgetService.Infrastructure.Persistence.PostgreSql;
using Npgsql;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer(
        (document, context, cancellationToken) =>
        {
            document.Servers =
            [
                new OpenApiServer
                {
                    Url = "/"
                }
            ];

            return Task.CompletedTask;
        });
});

var postgresPassword = builder.Configuration["Postgres:Password"];

if (string.IsNullOrWhiteSpace(postgresPassword))
{
    throw new InvalidOperationException(
        "PostgreSQL password is required.");
}

var postgresConnection = new NpgsqlConnectionStringBuilder
{
    Host = builder.Configuration["Postgres:Host"]
        ?? "budget-postgres-dev",
    Port = 5432,
    Database = builder.Configuration["Postgres:Database"]
        ?? "budgetdb",
    Username = builder.Configuration["Postgres:Username"]
        ?? "budgetuser",
    Password = postgresPassword
};

builder.Services.AddSingleton<NpgsqlDataSource>(_ =>
    NpgsqlDataSource.Create(postgresConnection.ConnectionString));

builder.Services.AddScoped<
    IBudgetRepository,
    PostgresBudgetRepository>();

builder.Services.AddScoped<CreateBudgetUseCase>();
builder.Services.AddScoped<GetAllBudgetsUseCase>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "BudgetService API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();