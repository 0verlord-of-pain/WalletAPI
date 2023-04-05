using MediatR;
using Wallet.Application.CQRS.Transactions.Queries.Views;

namespace Wallet.Application.CQRS.Transactions.Queries.GetTransaction;
public class GetUserTransactionsQuery : IRequest<IEnumerable<TransactionView>>
{
    public GetUserTransactionsQuery(Guid userId, int page)
    {
        UserId = userId;
        Page = page;
    }

    public Guid UserId { get; init; }
    public int Page { get; init; }

    
}