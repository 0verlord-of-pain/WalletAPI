using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.API.Infrastructure.Validators;
using Wallet.API.Persistence;
using Wallet.Application.CQRS.Users.Commands.DeleteUser;
using Wallet.Application.CQRS.Users.Commands.RestoreUser;
using Wallet.Application.CQRS.Users.Queries.GetAll;
using Wallet.Application.CQRS.Users.Queries.GetUser;
using Wallet.Application.CQRS.Users.Queries.Views;
using Wallet.Core.Exceptions;
using Wallet.Core.Extensions;
using Wallet.Domain.Entities;

namespace Wallet.API.Controllers;
public class AccountController : BaseController
{
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountController(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("signup")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> SignUp(
        [FromQuery] string email,
        [FromQuery] string userName,
        [FromQuery] string password)
    {
        if (!EmailValidator.IsValid(email)) throw new ValidationException("Email is not valid");

        if (!NameValidator.IsValid(userName)) throw new ValidationException("Surname is not valid");

        if (!await new PasswordValidator(_userManager).IsValidAsync(password))
            throw new ValidationException("Password is not valid");

        var user = await _userManager.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email.Equals(email));

        if (user != null)
        {
            if (user.IsArchived)
                throw new UserDeleteException(
                    "The user with this email has been deleted. If you would like to install it please contact support.");
            throw new ValidationException("User already exist");
        }

        user = Domain.Entities.User.Create(email, userName);

        var result = await _userManager.CreateAsync(user, password);

        if (result.TryGetErrors(out _)) throw new IdentityUserException("Failed to add user");

        var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

        if (addToRoleResult.TryGetErrors(out _)) throw new IdentityUserException("Failed to add role to user");

        return Created("", "");
    }

    [HttpPut("signin")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SignIn(
        [FromQuery] string email,
        [FromQuery] string password)
    {
        if (!EmailValidator.IsValid(email)) throw new ValidationException("Email is not valid");

        if (!await new PasswordValidator(_userManager).IsValidAsync(password))
            throw new ValidationException("Password is not valid");

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) throw new IdentityUserException("User with this email not found");

        if (user.IsArchived)
            throw new UserDeleteException(
                "The user with this email has been deleted. If you would like to install it please contact support.");
        var userName = user.UserName;

        var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

        if (!result.Succeeded) return BadRequest();

        return Ok();
    }

    [HttpPut("logout")]
    [Authorize(Policy = Policies.User)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(IEnumerable<UserView>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUsers([FromQuery] int? page = 1)
    {
        {
            var command = new GetAllUsersQuery(page.Value);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }

    [HttpGet("{userId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    [ProducesResponseType(typeof(UserView), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromRoute] Guid userId)
    {
        var command = new GetUserQuery().Create(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{userId}")]
    [Authorize(Policy = Policies.AdminOrManager)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        var command = new DeleteUserCommand(userId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPut("{userId}/restore")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [Authorize(Policy = Policies.AdminOrManager)]
    public async Task<IActionResult> Restore([FromRoute] Guid userId)
    {
        var command = new RestoreUserCommand(userId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}