using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Transactions.Queries.GetTransactionsByUserId;

public sealed class GetTransactionsByUserIdPreProcessor : IRequestPreProcessor<GetTransactionsByUserIdQuery>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public GetTransactionsByUserIdPreProcessor(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task Process(GetTransactionsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");

        var userPolicy = await _userManager.GetRolesAsync(user);
        if (!userPolicy.Contains(Core.Enums.Roles.Admin.ToString())
            && !userPolicy.Contains(Core.Enums.Roles.Manager.ToString()))
            throw new ForbidException("You do not have permission to do this");

        if (request.Page <= 0) throw new ValidationException("Page cannot be less than or equal to 0");
    }
}