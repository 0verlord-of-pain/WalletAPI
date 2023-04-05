using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;

namespace Wallet.Storage.Mapping;

public sealed class TransactionMap : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transaction");
        builder.HasKey(item => item.Id);
        builder.HasQueryFilter(i => !i.IsArchived);

        builder
            .HasIndex(item => item.Id)
            .IsUnique();

        builder
            .HasOne(item => item.User)
            .WithMany(item => item.Transactions)
            .HasForeignKey(item => item.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}