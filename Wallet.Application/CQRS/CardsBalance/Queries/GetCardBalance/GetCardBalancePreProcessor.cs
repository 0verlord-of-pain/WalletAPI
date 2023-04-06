using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalance;

public sealed class GetCardBalancePreProcessor : IRequestPreProcessor<GetCardBalanceQuery>
{
    private readonly DataContext _context;

    public GetCardBalancePreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(GetCardBalanceQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");

        var cardBalance = await _context.CardsBalance
            .FirstOrDefaultAsync(i => i.UserId == request.UserId, cancellationToken);

        if (cardBalance is null) throw new NotFoundException("CardBalance was not found");
    }
}