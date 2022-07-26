using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTPriceHistoryConfiguration : IEntityTypeConfiguration<NFTPriceHistory>
    {
        public void Configure(EntityTypeBuilder<NFTPriceHistory> builder)
        {
            builder.ToTable("nft_price_history", "eart");

            builder.HasNoKey();

            builder.Property(e => e.ParentId)
                .HasColumnName("parent_id");

            builder.Property(e => e.Price)
                .HasColumnName("price");

            builder.Property(e => e.DateOfPrice)
                .HasColumnName("date_of_price");

            //builder.HasOne(d => d.NFT)
            //    .WithMany(p => p.PriceHistory)
            //    .HasForeignKey(d => d.ParentId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
