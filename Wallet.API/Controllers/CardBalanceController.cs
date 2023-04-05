using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Persistence;
using Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalance;
using Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalanceById;
using Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalanceByUserId;
using Wallet.Application.CQRS.CardsBalance.Queries.Views;

namespace Wallet.API.Controllers;
public class CardBalanceController : BaseController
{
    [HttpGet("users/{userId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(CardBalanceView), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCardBalanceByUserId([FromRoute] Guid userId)
    {
        var result = await _mediator.Send(new GetCardBalanceByUserIdQuery(userId, UserId));
        return Ok(result);
    }

    [HttpGet("{cardBalanceId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(CardBalanceView), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCardBalanceById([FromRoute] Guid cardBalanceId)
    {
        var result = await _mediator.Send(new GetCardBalanceByIdQuery(cardBalanceId, UserId));
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(CardBalanceView), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var result = await _mediator.Send(new GetCardBalanceQuery(UserId));
        return Ok(result);
    }
}