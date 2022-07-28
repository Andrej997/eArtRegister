using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class BidPriceConfiguration : IEntityTypeConfiguration<BidPrice>
    {
        public void Configure(EntityTypeBuilder<BidPrice> builder)
        {
            builder.ToTable("bid_price", "eart");

            builder.HasKey(t => new { t.UserId, t.NFTId })
                .HasName("bid_price_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.Bid)
                .HasColumnName("bid");

            builder.Property(e => e.BidDate)
                .HasColumnName("bid_date");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Bids)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.NFT)
                .WithMany(p => p.Bids)
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
