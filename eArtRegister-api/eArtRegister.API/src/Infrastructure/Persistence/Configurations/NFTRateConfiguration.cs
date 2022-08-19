using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTRateConfiguration : IEntityTypeConfiguration<NFTRate>
    {
        public void Configure(EntityTypeBuilder<NFTRate> builder)
        {
            builder.ToTable("nft_rate", "eart");

            builder.HasKey(t => new { t.UserId, t.NFTId })
                .HasName("nft_rate_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.Rate)
                .HasColumnName("rate");

            builder.Property(e => e.DateOfRate)
                .HasColumnName("date_of_rate");

            builder.Property(e => e.Comment)
                .HasColumnName("comment");

            builder.HasOne(d => d.User)
                .WithMany(p => p.NFTRates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.NFT)
                .WithMany(p => p.Rates)
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
