using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class FollowNFTConfiguration : IEntityTypeConfiguration<FollowNFT>
    {
        public void Configure(EntityTypeBuilder<FollowNFT> builder)
        {
            builder.ToTable("follow_nft", "eart");

            builder.HasKey(t => new { t.UserId, t.NFTId })
                .HasName("follow_nft_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.FollowDate)
                .HasColumnName("follow_date");

            builder.HasOne(d => d.User)
                .WithMany(p => p.FollowingNFTs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.NFT)
                .WithMany(p => p.Followers)
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
