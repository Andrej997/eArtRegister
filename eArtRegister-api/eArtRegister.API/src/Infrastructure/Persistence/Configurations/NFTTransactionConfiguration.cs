using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTTransactionConfiguration : IEntityTypeConfiguration<NFTTransaction>
    {
        public void Configure(EntityTypeBuilder<NFTTransaction> builder)
        {
            builder.ToTable("nft_transaction", "eart");

            builder.HasKey(t => t.Id)
                .HasName("nft_transaction_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.FromWallet)
                .HasColumnName("from_wallet");

            builder.Property(e => e.ToWallet)
                .HasColumnName("to_wallet");

            builder.Property(e => e.DateOfTransaction)
                .HasColumnName("date_of_transaction");

            builder.Property(e => e.TransactionHash)
                .HasColumnName("transaction_hash");

            builder.HasOne(d => d.NFT)
               .WithMany(p => p.Transactions)
               .HasForeignKey(d => d.NFTId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
