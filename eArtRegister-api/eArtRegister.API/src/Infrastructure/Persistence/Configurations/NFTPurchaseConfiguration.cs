using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTPurchaseConfiguration : IEntityTypeConfiguration<NFTPurchase>
    {
        public void Configure(EntityTypeBuilder<NFTPurchase> builder)
        {
            builder.ToTable("nft_purchase", "eart");

            builder.HasKey(t => t.Address)
                .HasName("nft_purchase_pkey");

            builder.Property(e => e.Address)
                .HasColumnName("address")
                .IsRequired();

            builder.Property(e => e.Contract)
                .HasColumnName("contract");

            builder.Property(e => e.Abi)
                .HasColumnName("abi");

            builder.Property(e => e.Bytecode)
                .HasColumnName("bytecode");

            builder.Property(e => e.NFTId)
                .HasColumnName("nft_id");

            builder.Property(e => e.EntireAmount)
                .HasColumnName("entire_amount");

            builder.Property(e => e.RepaymentInInstallments)
               .HasColumnName("repayment_in_installments");

            builder.Property(e => e.Auction)
               .HasColumnName("auction");

            builder.Property(e => e.AmountInETH)
                .HasDefaultValue(0)
                .HasColumnName("amount_in_eth");

            builder.Property(e => e.DaysOnSale)
                .HasDefaultValue(1)
                .HasColumnName("days_on_sale");

            builder.Property(e => e.MinParticipation)
                .HasDefaultValue(0)
                .HasColumnName("min_participation");

            builder.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

            builder.Property(e => e.CreatedOn)
                .HasColumnName("created_on");

            builder.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");

            builder.Property(e => e.ModifiedOn)
                .HasColumnName("modified_on");

            builder.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

            builder.HasOne(d => d.NFT)
                .WithMany(p => p.PurchaseContracts)
                .HasForeignKey(d => d.NFTId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
