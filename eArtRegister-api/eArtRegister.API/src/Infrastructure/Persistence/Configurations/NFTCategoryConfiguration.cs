using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTCategoryConfiguration : IEntityTypeConfiguration<NFTCategory>
    {
        public void Configure(EntityTypeBuilder<NFTCategory> builder)
        {
            builder.ToTable("nft_category", "eart");

            builder.HasKey(e => new { e.NFTId, e.CategoryId });

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.CategoryId)
                .HasColumnName("category_id");

            builder.HasOne(d => d.NFT)
                .WithMany(p => p.Categories)
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Category)
                .WithMany()
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
