using MediatR;
using Wallet.Application.CQRS.Transactions.Queries.Views;

namespace Wallet.Application.CQRS.Transactions.Queries.GetTransactionById;
public class GetTransactionByIdQuery : IRequest<TransactionView>
{
    public GetTransactionByIdQuery(Guid userId, Guid transactionId)
    {
        UserId = userId;
        TransactionId = transactionId;
    }

    public Guid UserId { get; init; }
    public Guid TransactionId { get; init; }
}