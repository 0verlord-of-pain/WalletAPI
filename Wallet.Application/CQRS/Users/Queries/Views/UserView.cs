using Wallet.Domain.Entities;

namespace Wallet.Application.CQRS.Users.Queries.Views;
public class UserView
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string NickName { get; set; }
    public Guid CardBalanceId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime UpdatedOnUtc { get; set; }
    public bool IsArchived { get; set; }
    public string?[] Roles { get; set; }
}