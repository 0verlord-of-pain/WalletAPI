using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Persistence;

namespace Wallet.API.Controllers;
public class InfoController : BaseController
{
    [HttpGet("payment")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GetPayment()
    {
        var result = $"You’ve paid your {DateTime.UtcNow.Month:MMMM} balance.";
        return Ok(result);
    }

    [HttpGet("points")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public IActionResult GetDailyPoint()
    {
        var result = $"You’ve paid your {DateTime.UtcNow.Month:MMMM} balance.";
        return Ok(result);
    }
}