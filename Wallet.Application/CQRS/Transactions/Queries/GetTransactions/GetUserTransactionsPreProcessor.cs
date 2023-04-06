using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Transactions.Queries.GetTransactions;

public sealed class GetUserTransactionsPreProcessor : IRequestPreProcessor<GetUserTransactionsQuery>
{
    private readonly DataContext _context;

    public GetUserTransactionsPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(GetUserTransactionsQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");

        if (request.Page <= 0) throw new ValidationException("Page cannot be less than or equal to 0");
    }
}