using ContractingService.Application.Commands;
using ContractingService.Application.DTOs;
using ContractingService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContractingService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContractsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContractsController(IMediator mediator) =>
        _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ContractResponse>> CreateContract([FromBody] CreateContractRequest request)
    {
        var command = new CreateContractCommand(request.ProposalId);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetAllContracts), result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContractResponse>>> GetAllContracts()
    {
        var query = new GetAllContractsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
