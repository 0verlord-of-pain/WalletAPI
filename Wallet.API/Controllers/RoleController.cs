using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Persistence;
using Wallet.Application.CQRS.Roles.Commands.Attach;
using Wallet.Application.CQRS.Roles.Commands.Remove;
using Wallet.Application.CQRS.Users.Queries.Views;
using Wallet.Core.Enums;

namespace Wallet.API.Controllers;

public class RoleController : BaseController
{
    [HttpPut("{userId}/role/{role}/attach")]
    [Authorize(Policy = Policies.Admin)]
    [ProducesResponseType(typeof(UserView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AttachRole([FromRoute] Guid userId, [FromRoute] Roles role)
    {
        var result = await _mediator.Send(new AttachRoleCommand(userId, role));
        return Ok(result);
    }

    [HttpPut("{userId}/role/{role}/remove")]
    [Authorize(Policy = Policies.Admin)]
    [ProducesResponseType(typeof(UserView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveRole([FromRoute] Guid userId, [FromRoute] Roles role)
    {
        var result = await _mediator.Send(new RemoveRoleCommand(userId, role));
        return Ok(result);
    }
}