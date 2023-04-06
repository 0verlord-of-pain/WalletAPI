using MediatR;
using Wallet.Application.CQRS.CardsBalance.Queries.Views;

namespace Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalanceByUserId;

public class GetCardBalanceByUserIdQuery : IRequest<CardBalanceView>
{
    public GetCardBalanceByUserIdQuery(Guid whoBalanceCardUserId, Guid userId)
    {
        WhoBalanceCardUserId = whoBalanceCardUserId;
        UserId = userId;
    }

    public Guid WhoBalanceCardUserId { get; init; }
    public Guid UserId { get; init; }
}