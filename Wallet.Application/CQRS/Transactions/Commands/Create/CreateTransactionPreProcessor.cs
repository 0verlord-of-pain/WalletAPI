using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using Wallet.Core.Enums;
using Wallet.Core.Exceptions;
using Wallet.Domain.Entities;
using Wallet.Storage.Persistence;

namespace Wallet.Application.CQRS.Transactions.Commands.Create;

public sealed class CreateTransactionPreProcessor : IRequestPreProcessor<CreateTransactionCommand>
{
    private readonly DataContext _context;
    private readonly UserManager<User> _userManager;

    public CreateTransactionPreProcessor(UserManager<User> userManager, DataContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task Process(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new NotFoundException("User was not found");

        if (request.Amount <= 0m) throw new ValidationException("Amount cannot be less than or equal to 0");

        if (string.IsNullOrEmpty(request.Details)) throw new ValidationException("The Details field cannot be empty");

        if (!await IsImage(request.ImageUrl)) throw new ValidationException("Image link is incorrect");

        var cardBalance = await _context.CardsBalance
            .FirstOrDefaultAsync(i => i.Id == request.CardBalanceId, cancellationToken);

        if (cardBalance is null) throw new NotFoundException("Card Balance was not found");

        switch (request.Type)
        {
            case TransactionType.Credit:
                if (cardBalance.Balance - request.Amount <= 0) throw new ArgumentException("Insufficient balance");
                break;
            case TransactionType.Payment:
                if (cardBalance.Balance + request.Amount > cardBalance.Limit)
                    throw new ArgumentException("Limit exceeded");
                break;
        }
    }

    private static async Task<bool> IsImage(string url)
    {
        var isValid = false;
        try
        {
            var client = new HttpClient();

            var res = await client.GetAsync(url);

            isValid = res.ToString().Contains("Content-Type: image");
        }
        catch
        {
        }

        return isValid;
    }
}