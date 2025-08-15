using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProposalService.Application.Commands;
using ProposalService.Application.DTOs;
using ProposalService.Application.Queries;

namespace ProposalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProposalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProposalsController(IMediator mediator) =>
        _mediator = mediator;

    [HttpPost]
    public async Task<ActionResult<ProposalResponse>> CreateProposal([FromBody] CreateProposalRequest request)
    {
        var command = new CreateProposalCommand(request.CustomerName, request.InsuranceType, request.Value);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProposal), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProposalResponse>> GetProposal(Guid id)
    {
        var query = new GetProposalByIdQuery(id);
        var result = await _mediator.Send(query);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProposalResponse>>> GetAllProposals()
    {
        var query = new GetAllProposalsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<ActionResult<ProposalResponse>> UpdateProposalStatus(Guid id, [FromBody] UpdateProposalStatusRequest request)
    {
        var command = new UpdateProposalStatusCommand(id, request.Status);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
