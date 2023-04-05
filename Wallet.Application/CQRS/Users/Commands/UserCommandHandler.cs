using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.CQRS.Users.Commands.DeleteUser;
using Wallet.Application.CQRS.Users.Commands.RestoreUser;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Users.Commands;
public sealed class UserCommandHandler :
    IRequestHandler<DeleteUserCommand, Unit>,
    IRequestHandler<RestoreUserCommand, Guid>
{
    private readonly DataContext _context;

    public UserCommandHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        user?.SoftDelete();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task<Guid> Handle(
        RestoreUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        user?.Restore();
        await _context.SaveChangesAsync(cancellationToken);
        return user.Id;
    }
}