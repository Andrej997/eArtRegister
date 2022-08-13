using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTSaleConfiguration : IEntityTypeConfiguration<NFTSale>
    {
        public void Configure(EntityTypeBuilder<NFTSale> builder)
        {
            builder.ToTable("nft_sale", "eart");

            builder.HasKey(t => t.Id)
                .HasName("nft_sale_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.WalletSet)
                .HasColumnName("wallet_set");

            builder.Property(e => e.WalletBought)
                .HasColumnName("wallet_bought");

            builder.Property(e => e.DateOfSet)
                .HasColumnName("date_of_set");

            builder.Property(e => e.DateOfPurchase)
                .HasColumnName("date_of_purchase");

            builder.Property(e => e.TransactionHashSet)
                .HasColumnName("transaction_hash_set");

            builder.Property(e => e.TransactionHashPurchase)
                .HasColumnName("transaction_hash_purchase");

            builder.Property(e => e.SaleContractAddress)
                .HasColumnName("Sale_contract_address");

            builder.HasOne(d => d.NFT)
               .WithMany(p => p.Sales)
               .HasForeignKey(d => d.NFTId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
