using Wallet.Core.Enums;

namespace Wallet.Domain.Entities;

public class Transaction : IBaseEntity
{
    public TransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public string Name { get; set; }
    public string Details { get; set; }
    public TransactionStatus Status { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string ImageUrl { get; set; }
    public Guid Id { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }

    public void SoftDelete()
    {
        IsArchived = true;
    }

    public void Restore()
    {
        IsArchived = false;
    }

    public static Transaction Create(
        TransactionType type,
        decimal amount,
        string name,
        string details,
        TransactionStatus status,
        User user,
        string imageUrl)
    {
        return new Transaction
        {
            Type = type,
            Amount = amount,
            Name = name,
            Details = details,
            Status = status,
            User = user,
            UserId = user.Id,
            ImageUrl = imageUrl
        };
    }
}