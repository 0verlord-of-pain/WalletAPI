using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;
using Wallet.Storage.Extensions;
using Wallet.Storage.Mapping;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardBalance>().HasQueryFilter(i => !i.IsArchived);
        modelBuilder.Entity<Transaction>().HasQueryFilter(i => !i.IsArchived);
        modelBuilder.Entity<User>().HasQueryFilter(i => !i.IsArchived);

        modelBuilder.Entity<CardBalance>()
            .HasOne(item => item.User)
            .WithOne(item => item.CardBalance)
            .HasForeignKey<CardBalance>(item=>item.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}