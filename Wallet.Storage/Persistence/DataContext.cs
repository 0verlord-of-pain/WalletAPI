using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Storage.Extensions;

namespace Wallet.Storage.Persistence;
public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public DataContext(
        DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<CardBalance> CardsBalance { get; set; }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        this.UpdateSystemDates();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}