using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class PriceOfferConfiguration : IEntityTypeConfiguration<PriceOffer>
    {
        public void Configure(EntityTypeBuilder<PriceOffer> builder)
        {
            builder.ToTable("price_offer", "eart");

            builder.HasKey(t => new { t.UserId, t.NFTId })
                .HasName("price_offer_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.Offer)
                .HasColumnName("offer");

            builder.Property(e => e.DateOfOffer)
                .HasColumnName("date_of_offer");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Offers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.NFT)
                .WithMany(p => p.PriceOffers)
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
