using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.CQRS.Transactions.Commands.Create;
using Wallet.Application.CQRS.Transactions.Commands.Delete;
using Wallet.Application.CQRS.Transactions.Queries.Views;
using Wallet.Core.Enums;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Transactions.Commands;

public class TransactionCommandHandler :
    IRequestHandler<CreateTransactionCommand, TransactionView>,
    IRequestHandler<DeleteTransactionCommand, Unit>
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public TransactionCommandHandler(UserManager<User> userManager, IMapper mapper, DataContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<TransactionView> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());

        var cardBalance =
            await _context.CardsBalance.FirstOrDefaultAsync(i => i.Id == request.CardBalanceId, cancellationToken);

        var transaction = Transaction.Create(request.Type, request.Amount, user.UserName, request.Details,
            request.Status, user, request.ImageUrl);

        _context.Transactions.Add(transaction);

        switch (request.Type)
        {
            case TransactionType.Credit:
                cardBalance?.ChangeBalance(-request.Amount);
                break;
            case TransactionType.Payment:
                cardBalance?.ChangeBalance(request.Amount);
                break;
        }

        await _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<TransactionView>(transaction);

        return result;
    }

    public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = _context.Transactions
            .FirstOrDefault(i => i.Id == request.TransactionId);

        transaction?.SoftDelete();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}