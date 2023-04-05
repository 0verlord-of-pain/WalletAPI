using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Entities;

namespace Wallet.Storage.Mapping;
public sealed class CardBalanceMap : IEntityTypeConfiguration<CardBalance>
{
    public void Configure(EntityTypeBuilder<CardBalance> builder)
    {
        builder.ToTable("CardBalance");
        builder.HasKey(item => item.Id);
        builder.HasQueryFilter(i => !i.IsArchived);

        builder
            .HasIndex(item => item.Id)
            .IsUnique();

        builder
            .HasOne(item => item.User)
            .WithOne(item => item.CardBalance)
            .HasForeignKey<CardBalance>(item => item.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}