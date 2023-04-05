using Wallet.Core.Enums;

namespace Wallet.Application.CQRS.Transactions.Queries.Views;
public class TransactionView
{
    public Guid Id { get; set; }
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public TransactionStatus Status { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string ImageUrl { get; set; }
    public string CreatedOnUtc { get; set; }
}