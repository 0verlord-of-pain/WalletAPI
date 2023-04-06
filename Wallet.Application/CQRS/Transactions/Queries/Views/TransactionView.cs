using Wallet.Core.Enums;

namespace Wallet.Application.CQRS.Transactions.Queries.Views;

public class TransactionView
{
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public string Amount { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public TransactionStatus Status { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CreatedOnUtc { get; set; } = string.Empty;
}