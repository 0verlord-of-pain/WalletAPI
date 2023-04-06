using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wallet.Domain.Entities;

namespace Wallet.Storage.Mapping;

public sealed class CardBalanceMap : IEntityTypeConfiguration<CardBalance>
{
    public void Configure(EntityTypeBuilder<CardBalance> builder)
    {
        builder.ToTable("CardBalance");
        builder.HasKey(item => item.Id);

        builder
            .HasIndex(item => item.Id)
            .IsUnique();
    }
}