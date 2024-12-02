using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubscriptionApi.Common;
using SubscriptionApi.Features.Subscriptions.Commands;
using SubscriptionApi.Features.Subscriptions.Queries;

namespace SubscriptionApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("subscribe")]
    [Idempotency(30)]
    public async Task<IActionResult> Subscribe(
        [FromBody] CreateSubscriptionCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpPost("unsubscribe")]
    [Idempotency(30)]
    public async Task<IActionResult> Unsubscribe(
        [FromBody] UnsubscribeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok() : NotFound(result.Error);
    }

    [HttpGet("subscription-summary")]
    public async Task<IActionResult> GetSubscriptionSummary(
        [FromQuery] string customerPhoneNumber)
    {
        var query = new GetSubscriptionSummaryQuery(customerPhoneNumber);
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}