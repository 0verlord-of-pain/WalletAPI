using MediatR;

namespace Wallet.Application.CQRS.Transactions.Commands.Delete;

public class DeleteTransactionCommand : IRequest<Unit>
{
    public DeleteTransactionCommand(
        Guid userId,
        Guid transactionId)
    {
        UserId = userId;
        TransactionId = transactionId;
    }

    public Guid UserId { get; init; }
    public Guid TransactionId { get; init; }
}