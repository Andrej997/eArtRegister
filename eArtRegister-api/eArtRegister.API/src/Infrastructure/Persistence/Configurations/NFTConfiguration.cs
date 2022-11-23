using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class NFTConfiguration : IEntityTypeConfiguration<NFT>
    {
        public void Configure(EntityTypeBuilder<NFT> builder)
        {
            builder.ToTable("nft", "eart");

            builder.HasKey(t => t.Id)
                .HasName("nft_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.IPFSImageHash)
                .HasColumnName("ipfs_image_hash");

            builder.Property(e => e.IPFSNFTHash)
                .HasColumnName("ipfs_nft_hash");

            builder.Property(e => e.IPFSImageSize)
                .HasColumnName("ipfs_image_size");

            builder.Property(e => e.IPFSNFTSize)
                .HasColumnName("ipfs_nft_size");

            builder.Property(e => e.TokenId)
                .HasColumnName("token_id");

            builder.Property(e => e.StatusId)
               .HasColumnName("status_id");

            builder.Property(e => e.BundleId)
               .HasColumnName("bundle_id");

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

            builder.Property(e => e.TransactionHash)
                .HasColumnName("transaction_hash");

            builder.Property(e => e.TokenStandard)
                .HasColumnName("token_standard");

            builder.HasOne(d => d.Status)
                .WithMany()
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Bundle)
                .WithMany(p => p.NFTs)
                .HasForeignKey(d => d.BundleId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.AfterUpdate(trigger => trigger
            //    .Action(action => action
            //    .Condition((transactionBeforeUpdate, transactionAfterUpdate) => transactionBeforeUpdate.CurrentPrice != transactionAfterUpdate.CurrentPrice)
            //    .Insert((oldValues, newValues) => new NFTPriceHistory()
            //    {
            //        ParentId = oldValues.Id,
            //        Price = oldValues.CurrentPrice,
            //        DateOfPrice = oldValues.CurrentPriceDate
            //    })));
        }
    }
}
