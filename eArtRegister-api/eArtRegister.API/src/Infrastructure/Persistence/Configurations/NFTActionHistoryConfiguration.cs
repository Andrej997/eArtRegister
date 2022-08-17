using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTActionHistoryConfiguration : IEntityTypeConfiguration<NFTActionHistory>
    {
        public void Configure(EntityTypeBuilder<NFTActionHistory> builder)
        {
            builder.ToTable("nft_action_history", "eart");

            builder.HasKey(t => t.Id)
                .HasName("nft_action_history_pkey");

            builder.Property(e => e.Id)
               .HasColumnName("id")
               .HasColumnType("uuid")
               .HasDefaultValueSql("uuid_generate_v4()")
               .IsRequired();

            builder.Property(e => e.Wallet)
                .HasColumnName("wallet");

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.EventAction)
                .HasColumnName("event_action");

            builder.Property(e => e.EventTimestamp)
                .HasColumnName("event_timestamp");

            builder.Property(e => e.IsCompleted)
                .HasColumnName("is_completed");

            builder.HasOne(d => d.NFT)
                .WithMany()
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
