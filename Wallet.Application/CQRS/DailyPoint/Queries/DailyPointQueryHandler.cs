using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.CQRS.DailyPoint.Queries.GetDailyPoint;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.DailyPoint.Queries;

public sealed class DailyPointQueryHandler : IRequestHandler<GetDailyPointQuery, string>
{
    private readonly DataContext _context;

    public DailyPointQueryHandler(DataContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(
        GetDailyPointQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(i => i.Id == request.UserId, cancellationToken);

        var points = CalculatePoints(user.CreatedOnUtc);
        var result = "";

        if (points > 1000m)
        {
            var roundedValue = Math.Ceiling(points / 1000) * 1000;
            result = (roundedValue / 1000).ToString("0K");
        }

        else result = Math.Round(points, 0).ToString();

        return result;
    }

    private decimal CalculatePoints(DateTime createdOnUtc)
    {
        var today = DateTime.Now.Date;
        var seasonStartDate = GetSeasonStartDate(createdOnUtc);

        if (today < seasonStartDate) return 0;
        if (today == seasonStartDate && today == createdOnUtc.Date) return 2;

        var months = (today.Year - seasonStartDate.Year) * 12 + (today.Month - seasonStartDate.Month);

        if (today.Day < seasonStartDate.Day) months--;

        var points = 0.0m;
        var valueLastDay = 0m;
        var valueTwoDaysAgo = 0m;

        for (var m = 0; m <= months; m += 3)
        {
            seasonStartDate = GetSeasonStartDate(createdOnUtc.AddMonths(m));
            var daysToCharge = (today - seasonStartDate).Days + 1;
            for (var i = 1; i <= daysToCharge; i++)
                switch (i)
                {
                    case 1:
                        points += 2m;
                        valueTwoDaysAgo = 2m;
                        break;
                    case 2:
                        points += 3m;
                        valueLastDay = 3m;
                        break;
                    default:
                    {
                        var addPoints = valueTwoDaysAgo + 0.6m * valueLastDay;
                        points += addPoints;
                        valueTwoDaysAgo = valueLastDay;
                        valueLastDay = addPoints;
                        break;
                    }
                }
        }

        return points;
    }

    private DateTime GetSeasonStartDate(DateTime date)
    {
        var year = date.Year;

        if (date <= new DateTime(year, 12, 1)) return new DateTime(year, 12, 1).Date;
        if (date <= new DateTime(year, 3, 1)) return new DateTime(year, 3, 1).Date;

        return date <= new DateTime(year, 6, 1) ? new DateTime(year, 6, 1).Date : new DateTime(year, 9, 1).Date;
    }
}