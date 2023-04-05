﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Wallet.Application.CQRS.Transactions.Commands.Create;
using Wallet.Application.CQRS.Transactions.Commands.Delete;
using Wallet.Application.CQRS.Transactions.Queries.Views;
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

        var transaction = Transaction.Create(request.Type, request.Amount, request.Name, request.Details,
            request.Status, user, request.ImageUrl);

        _context.Transactions.Add(transaction);
        await  _context.SaveChangesAsync(cancellationToken);

        var result = _mapper.Map<TransactionView>(transaction);

        return result;
    }

    public async Task<Unit> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = _context.Transactions
            .FirstOrDefault(i=>i.Id == request.TransactionId);

        transaction?.SoftDelete();

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}