using MediatR.Pipeline;
using Microsoft.AspNetCore.Identity;
using Wallet.Core.Exceptions;
using Wallet.Domain.Entities;
using SkiaSharp;

namespace Wallet.Application.CQRS.Transactions.Commands.Create;
public sealed class CreateTransactionPreProcessor : IRequestPreProcessor<CreateTransactionCommand>
{
    private readonly UserManager<User> _userManager;

    public CreateTransactionPreProcessor(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Process(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new NotFoundException("User was not found");

        if (request.Amount <= 0m) throw new ValidationException("Amount cannot be less than or equal to 0");

        if (string.IsNullOrEmpty(request.Details)) throw new ValidationException("The Details field cannot be empty");

        if (string.IsNullOrEmpty(request.Name)) throw new ValidationException("The Name field cannot be empty");

        if (!await IsImage(request.ImageUrl)) throw new ValidationException("Image link is incorrect");
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