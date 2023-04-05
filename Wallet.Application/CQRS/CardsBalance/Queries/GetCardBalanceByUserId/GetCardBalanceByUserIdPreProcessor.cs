using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalanceByUserId;
public sealed class GetCardBalanceByUserIdPreProcessor : IRequestPreProcessor<GetCardBalanceByUserIdQuery>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public GetCardBalanceByUserIdPreProcessor(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task Process(GetCardBalanceByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");

        var cardBalance = await _context.CardsBalance
            .FirstOrDefaultAsync(i => i.UserId == request.WhoBalanceCardUserId, cancellationToken);

        if (cardBalance is null) throw new NotFoundException("CardBalance was not found");

        if (cardBalance.UserId != request.UserId)
        {
            var userPolicy = await _userManager.GetRolesAsync(user);
            if (!userPolicy.Contains(Core.Enums.Roles.Admin.ToString())
                && !userPolicy.Contains(Core.Enums.Roles.Manager.ToString()))
                throw new ForbidException("You do not have permission to do this");
        }
    }
}