using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.API.Controllers.In;
using Wallet.API.Persistence;
using Wallet.Application.CQRS.Transactions.Commands.Create;
using Wallet.Application.CQRS.Transactions.Commands.Delete;
using Wallet.Application.CQRS.Transactions.Queries.GetTransactionById;
using Wallet.Application.CQRS.Transactions.Queries.GetTransactions;
using Wallet.Application.CQRS.Transactions.Queries.GetTransactionsByUserId;
using Wallet.Application.CQRS.Transactions.Queries.Views;

namespace Wallet.API.Controllers;
public class TransactionController : BaseController
{
    [HttpGet]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserTransactions([FromQuery] int? page = 1)
    {
        var command = new GetUserTransactionsQuery(UserId, page.Value);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{transactionId}")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransaction([FromRoute] Guid transactionId)
    {
        var command = new GetTransactionByIdQuery(UserId, transactionId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("users/{userId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionsByUserId( [FromRoute] Guid userId, [FromQuery] int? page = 1)
    {
        var command = new GetTransactionsByUserIdQuery(userId, page.Value);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("create")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(TransactionView), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create([FromBody] CreateTransactionModel model)
    {
        var command = new CreateTransactionCommand(
            UserId, 
            model.Type, 
            model.Amount, 
            model.Name, 
            model.Details, 
            model.Status, 
            model.ImageUrl);
        var result  = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{transactionId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] Guid transactionId)
    {
        var command = new DeleteTransactionCommand(UserId, transactionId);
        await _mediator.Send(command);
        return NoContent();
    }
}