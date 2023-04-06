using MediatR;
using Wallet.Application.CQRS.CardsBalance.Queries.Views;

namespace Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalanceById;

public class GetCardBalanceByIdQuery : IRequest<CardBalanceView>
{
    public GetCardBalanceByIdQuery(Guid cardBalanceId, Guid userId)
    {
        CardBalanceId = cardBalanceId;
        UserId = userId;
    }

    public Guid CardBalanceId { get; init; }
    public Guid UserId { get; init; }
}