using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTLikeConfiguration : IEntityTypeConfiguration<NFTLike>
    {
        public void Configure(EntityTypeBuilder<NFTLike> builder)
        {
            builder.ToTable("nft_like", "eart");

            builder.HasKey(t => new { t.UserId, t.NFTId })
                .HasName("nft_like_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.LikedDate)
                .HasColumnName("liked_date");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Liked)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.NFT)
                .WithMany(p => p.Likes)
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
