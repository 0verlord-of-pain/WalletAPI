using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Wallet.API.Persistence;
using Wallet.Application.CQRS.DailyPoint.Queries.GetDailyPoint;

namespace Wallet.API.Controllers;

public class InfoController : BaseController
{
    [HttpGet("payment")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GetPayment()
    {
        var result = $"You’ve paid your {DateTime.UtcNow.ToString("MMMM", new CultureInfo("en-GB"))} balance.";
        return Ok(result);
    }

    [HttpGet("points")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDailyPoint()
    {
        var result = await _mediator.Send(new GetDailyPointQuery(UserId));
        return Ok(result);
    }
}