using Wallet.Core.Enums;

namespace Wallet.API.Controllers.In;

public class CreateTransactionModel
{
    public Guid CardBalanceId { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Details { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}