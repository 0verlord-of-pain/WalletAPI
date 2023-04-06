namespace Wallet.Application.CQRS.CardsBalance.Queries.Views;

public class CardBalanceView
{
    public Guid Id { get; set; }
    public int Limit { get; set; }
    public decimal Balance { get; set; }
    public decimal Available => Limit - Balance;
    public Guid UserId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}