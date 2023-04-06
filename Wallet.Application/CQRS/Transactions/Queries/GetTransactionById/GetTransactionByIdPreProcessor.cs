using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Transactions.Queries.GetTransactionById;

public sealed class GetTransactionByIdPreProcessor : IRequestPreProcessor<GetTransactionByIdQuery>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public GetTransactionByIdPreProcessor(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task Process(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");

        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(i => i.Id == request.TransactionId, cancellationToken);

        if (transaction is null) throw new NotFoundException("Transaction was not found");

        if (transaction.UserId != request.UserId)
        {
            var userPolicy = await _userManager.GetRolesAsync(user);
            if (!userPolicy.Contains(Core.Enums.Roles.Admin.ToString())
                && !userPolicy.Contains(Core.Enums.Roles.Manager.ToString()))
                throw new ForbidException("You do not have permission to do this");
        }
    }
}