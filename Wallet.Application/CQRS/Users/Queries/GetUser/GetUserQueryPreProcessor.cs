using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Users.Queries.GetUser;

public sealed class GetUserQueryPreProcessor : IRequestPreProcessor<GetUserQuery>
{
    private readonly DataContext _context;

    public GetUserQueryPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");
    }
}