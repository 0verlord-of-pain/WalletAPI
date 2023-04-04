using MediatR;
using Wallet.Application.CQRS.Users.Queries.Views;

namespace Wallet.Application.CQRS.Users.Queries.GetUser;
public class GetUserQuery : IRequest<UserView>
{
    public Guid UserId { get; init; }

    public GetUserQuery Create(Guid userId)
    {
        return new GetUserQuery
        {
            UserId = userId
        };
    }
}