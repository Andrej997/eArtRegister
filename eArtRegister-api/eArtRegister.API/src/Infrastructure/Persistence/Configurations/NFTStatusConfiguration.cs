using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTStatusConfiguration : IEntityTypeConfiguration<NFTStatus>
    {
        public void Configure(EntityTypeBuilder<NFTStatus> builder)
        {
            builder.ToTable("nft_status", "eart");

            builder.HasKey(t => t.Id)
                .HasName("nft_status_pkey");

            builder.HasData(
                new NFTStatus { Id = "MINTED" },
                new NFTStatus { Id = "ON_SALE" },
                new NFTStatus { Id = "NOT_ON_SALE" },
                new NFTStatus { Id = "PENDING" },
                new NFTStatus { Id = "SOLD" }
                );
        }
    }
}
