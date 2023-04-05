using MediatR;

namespace Wallet.Application.CQRS.DailyPoint.Queries.GetDailyPoint;
public class GetDailyPointQuery : IRequest<string>
{
    public GetDailyPointQuery(Guid userId)
    {
        UserId = userId;
    }
    
    public Guid UserId { get; init; }
}