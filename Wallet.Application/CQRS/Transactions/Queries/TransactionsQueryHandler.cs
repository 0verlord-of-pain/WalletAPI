using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.CQRS.Transactions.Queries.GetTransaction;
using Wallet.Application.CQRS.Transactions.Queries.GetTransactionByUserId;
using Wallet.Application.CQRS.Transactions.Queries.Views;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Transactions.Queries;
public sealed class TransactionsQueryHandler :
    IRequestHandler<GetUserTransactionsQuery, IEnumerable<TransactionView>>,
    IRequestHandler<GetTransactionsByUserIdQuery, IEnumerable<TransactionView>>
{
    private const int Limit = 10;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public TransactionsQueryHandler(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<IEnumerable<TransactionView>> Handle(
        GetUserTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _context.Transactions
            .Where(i=>i.UserId == request.UserId)
            .Skip(Limit * (request.Page - 1))
            .Take(Limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<TransactionView>>(transactions);

        return result;
    }

    public async Task<IEnumerable<TransactionView>> Handle(
        GetTransactionsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await _context.Transactions
            .Where(i => i.UserId == request.UserId)
            .Skip(Limit * (request.Page - 1))
            .Take(Limit)
            .ToListAsync(cancellationToken);

        var result = _mapper.Map<IEnumerable<TransactionView>>(transactions);

        return result;
    }
}