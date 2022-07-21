using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTConfiguration : IEntityTypeConfiguration<NFT>
    {
        public void Configure(EntityTypeBuilder<NFT> builder)
        {
            builder.ToTable("nft", "eart");

            builder.HasKey(t => t.Id)
                .HasName("nft_pkey");

            builder.Property(e => e.OwnerId)
                .HasColumnName("owner_id");

            builder.Property(e => e.StatusId)
               .HasColumnName("status_id");

            builder.Property(e => e.CurrentPrice)
                .HasColumnName("current_price");

            builder.Property(e => e.CreatorId)
                .HasColumnName("creator_id");

            builder.Property(e => e.CreatorRoyality)
                .HasColumnName("creator_royality");

            builder.Property(e => e.CurrentPrice)
                .HasColumnName("current_price");

            builder.Property(e => e.MintedAt)
                .HasColumnName("minted_at");

            builder.Property(e => e.MintedPrice)
                .HasColumnName("minted_price");

            builder.HasOne(d => d.Status)
                .WithMany()
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Bundle)
                .WithMany(p => p.NFTs)
                .HasForeignKey(d => d.BundleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Owner)
                .WithMany(p => p.OwnedNFTs)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Creator)
                .WithMany(p => p.MintedNFTs)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
