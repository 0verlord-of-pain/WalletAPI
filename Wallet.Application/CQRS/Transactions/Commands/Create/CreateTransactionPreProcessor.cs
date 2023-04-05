using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Core.Exceptions;
using Wallet.Domain.Entities;
using SkiaSharp;
using Wallet.Storage.Persistence;
using Wallet.Core.Enums;

namespace Wallet.Application.CQRS.Transactions.Commands.Create;
public sealed class CreateTransactionPreProcessor : IRequestPreProcessor<CreateTransactionCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly DataContext _context;

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

        if (string.IsNullOrEmpty(request.Name)) throw new ValidationException("The Name field cannot be empty");

        if (!await IsImage(request.ImageUrl)) throw new ValidationException("Image link is incorrect");

        var cardBalance = await _context.CardsBalance
            .FirstOrDefaultAsync(i=>i.Id == request.CardBalanceId,cancellationToken);

        if(cardBalance is null) throw new NotFoundException("Card Balance was not found");

        switch (request.Type)
        {
            case TransactionType.Credit:
                if (cardBalance.Balance - request.Amount <= 0) throw new ArgumentException("Insufficient balance");
                break;
            case TransactionType.Payment:
                if (cardBalance.Balance + request.Amount > cardBalance.Limit) throw new ArgumentException("Limit exceeded");
                break;
        }
    }

    private static async Task<bool> IsImage(string url)
    {
        var isValid = false;
        try
        {
            using var client = new HttpClient();
            await using var stream = await client.GetStreamAsync(url);
            using var skiaStream = new SKManagedStream(stream);
            using var skBitmap = SKBitmap.Decode(skiaStream);
            if (skBitmap != null) isValid = true;
        }
        catch { }
        return isValid;
    }
}