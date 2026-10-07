using BudgetService.Application.Abstractions.Persistence;
using BudgetService.Application.UseCases.Budgets.Create;
using BudgetService.Application.UseCases.Budgets.GetAll;
using BudgetService.Infrastructure.Persistence.Json;
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

builder.Services
    .AddOptions<JsonStorageOptions>()
    .Bind(
        builder.Configuration.GetSection(
            JsonStorageOptions.SectionName))
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.FilePath),
        "JSON storage file path is required.")
    .ValidateOnStart();

builder.Services.AddSingleton<
    IBudgetRepository,
    JsonBudgetRepository>();

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