using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Wallet.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IMediator _mediator => HttpContext.RequestServices.GetService<IMediator>();

    internal Guid UserId => !User.Identity.IsAuthenticated
        ? Guid.Empty
        : Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
}