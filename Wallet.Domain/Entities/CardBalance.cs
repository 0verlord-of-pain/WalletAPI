namespace Wallet.Domain.Entities;

public class CardBalance : IBaseEntity
{
    public Guid Id { get; set; }
    public int Limit { get; set; } = 1500;
    public decimal Balance { get; set; } = 0;
    public decimal Available => Limit - Balance;
    public Guid UserId { get; set; }
    public User User { get; set; }
    public bool IsArchived { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }

    public static CardBalance Create(User user)
    {
        return new CardBalance
        {
            
            User = user,
            UserId = user.Id,
        };
    }


    public void ChangeLimit(int newLimit)
    {
        Limit = newLimit;
    }

    public void ChangeBalance(decimal amount)
    {
        Balance += amount;
    }

    public void SoftDelete()
    {
        IsArchived = true;
    }

    public void Restore()
    {
        IsArchived = false;
    }
}