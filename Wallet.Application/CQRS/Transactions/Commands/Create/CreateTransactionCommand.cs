using MediatR;
using Wallet.Application.CQRS.Transactions.Queries.Views;
using Wallet.Core.Enums;

namespace Wallet.Application.CQRS.Transactions.Commands.Create;

public class CreateTransactionCommand : IRequest<TransactionView>
{
    public CreateTransactionCommand(
        Guid userId,
        Guid cardBalanceId,
        TransactionType type,
        decimal amount,
        string details,
        TransactionStatus status,
        string imageUrl)
    {
        UserId = userId;
        CardBalanceId = cardBalanceId;
        Type = type;
        Amount = amount;
        Details = details;
        Status = status;
        ImageUrl = imageUrl;
    }

    public Guid UserId { get; init; }
    public Guid CardBalanceId { get; init; }
    public TransactionType Type { get; init; }
    public decimal Amount { get; init; }
    public string Details { get; init; }
    public TransactionStatus Status { get; init; }
    public string ImageUrl { get; init; }
}