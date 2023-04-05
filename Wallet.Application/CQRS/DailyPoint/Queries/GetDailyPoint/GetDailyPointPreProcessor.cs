using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.DailyPoint.Queries.GetDailyPoint;

public sealed class GetDailyPointPreProcessor : IRequestPreProcessor<GetDailyPointQuery>
{
    private readonly DataContext _context;

    public GetDailyPointPreProcessor(DataContext context)
    {
        _context = context;
    }

    public async Task Process(GetDailyPointQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        if (user is null) throw new NotFoundException("User was not found");
    }
}