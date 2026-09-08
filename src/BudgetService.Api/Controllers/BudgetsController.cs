using Microsoft.AspNetCore.Mvc;
using BudgetService.Application.UseCases.Budgets.Create;
using BudgetService.Application.UseCases.Budgets.GetAll;
using BudgetService.Api.Contracts.Budgets;

namespace BudgetService.Api.Controllers;

[ApiController]
[Route("api/budgets")]
public sealed class BudgetsController : ControllerBase
{
    private readonly CreateBudgetUseCase _createBudgetUseCase;
    private readonly GetAllBudgetsUseCase _getAllBudgetsUseCase;

    public BudgetsController(
    CreateBudgetUseCase createBudgetUseCase,
    GetAllBudgetsUseCase getAllBudgetsUseCase)
{
    _createBudgetUseCase = createBudgetUseCase;
    _getAllBudgetsUseCase = getAllBudgetsUseCase;
}

   [HttpPost]
   public async Task<ActionResult <BudgetResponse>> CreateAsync(
    CreateBudgetRequest request, 
    CancellationToken cancellationToken)
    {
        var command = new CreateBudgetCommand(request.Name, request.Amount);

        var result = await _createBudgetUseCase.ExecuteAsync(
            command,
            cancellationToken);

        var response  = new BudgetResponse(
            result.Id,
            result.Name,
            result.Amount,
            result.CreatedAt );

         return Created(
            $"/api/budgets/{response.Id}",
            response);   
        
    }

    [HttpGet]
public async Task<ActionResult<IReadOnlyCollection<BudgetResponse>>> GetAllAsync(
    CancellationToken cancellationToken)
{
    var results = await _getAllBudgetsUseCase.ExecuteAsync(
        cancellationToken);

    var response = results
        .Select(
            result => new BudgetResponse(
                result.Id,
                result.Name,
                result.Amount,
                result.CreatedAt))
        .ToArray();

    return Ok(response);
}
}
