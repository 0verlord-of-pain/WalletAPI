using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalance;
using Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalanceById;
using Wallet.Application.CQRS.CardsBalance.Queries.GetCardBalanceByUserId;
using Wallet.Application.CQRS.CardsBalance.Queries.Views;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.CardsBalance.Queries;

public sealed class CardsBalanceQueryHandler :
    IRequestHandler<GetCardBalanceQuery, CardBalanceView>,
    IRequestHandler<GetCardBalanceByIdQuery, CardBalanceView>,
    IRequestHandler<GetCardBalanceByUserIdQuery, CardBalanceView>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CardsBalanceQueryHandler(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<CardBalanceView> Handle(
        GetCardBalanceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var cardsBalance = await _context.CardsBalance
            .FirstOrDefaultAsync(i => i.Id == request.CardBalanceId, cancellationToken);

        var result = _mapper.Map<CardBalanceView>(cardsBalance);

        return result;
    }

    public async Task<CardBalanceView> Handle(
        GetCardBalanceByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var cardsBalance = await _context.CardsBalance
            .FirstOrDefaultAsync(i => i.UserId == request.UserId, cancellationToken);

        var result = _mapper.Map<CardBalanceView>(cardsBalance);

        return result;
    }

    public async Task<CardBalanceView> Handle(
        GetCardBalanceQuery request,
        CancellationToken cancellationToken)
    {
        var cardsBalance = await _context.CardsBalance
            .FirstOrDefaultAsync(i => i.UserId == request.UserId, cancellationToken);

        var result = _mapper.Map<CardBalanceView>(cardsBalance);

        return result;
    }
}