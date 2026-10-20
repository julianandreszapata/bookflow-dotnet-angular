using BookFlow.Application.Contracts;
using BookFlow.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookFlow.Api.Controllers;

[ApiController]
[Route("api/loans")]
public sealed class LoansController(LoanService service) : ControllerBase
{
    [HttpGet]
    public Task<IReadOnlyList<LoanResponse>> GetAll(CancellationToken ct) => service.GetAllAsync(ct);

    [HttpGet("{id:guid}")]
    public Task<LoanResponse> GetById(Guid id, CancellationToken ct) => service.GetByIdAsync(id, ct);

    [HttpPost]
    public async Task<ActionResult<LoanResponse>> Create(CreateLoanRequest request, CancellationToken ct)
    {
        var loan = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
    }

    [HttpPatch("{id:guid}/return")]
    public Task<LoanResponse> Return(Guid id, CancellationToken ct) => service.ReturnAsync(id, ct);
}
