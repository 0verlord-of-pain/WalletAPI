using MediatR;
using Wallet.Application.CQRS.CardsBalance.Queries.Views;

namespace Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalance;
public class GetCardBalanceQuery : IRequest<CardBalanceView>
{
    public GetCardBalanceQuery(Guid userId)
    {
        UserId = userId;
    }

    public Guid UserId { get; init; }
}
