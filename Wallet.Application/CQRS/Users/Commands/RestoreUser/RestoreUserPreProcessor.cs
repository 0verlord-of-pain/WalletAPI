using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Users.Commands.RestoreUser;
public sealed class RestoreUserPreProcessor : IRequestPreProcessor<RestoreUserCommand>
{
    private readonly DataContext _context;

    public RestoreUserPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(RestoreUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);
        if (user is null) throw new NotFoundException("User was not found");
    }
}