using eArtRegister.API.Domain.Entities;
using Laraue.EfCoreTriggers.Common.Extensions;
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

            builder.Property(e => e.IPFSId)
                .HasColumnName("ipfs_id");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Description)
                .HasColumnName("description");

            builder.Property(e => e.Order)
                .HasColumnName("order");

            builder.Property(e => e.StatusId)
               .HasColumnName("status_id");

            builder.Property(e => e.BundleId)
               .HasColumnName("bundle_id");

            builder.Property(e => e.CreatorId)
                .HasColumnName("creator_id");

            builder.Property(e => e.MintedAt)
               .HasColumnName("minted_at");

            builder.Property(e => e.CurrentPrice)
                .HasColumnName("current_price");

            builder.Property(e => e.CurrentPriceDate)
                .HasColumnName("current_price_date");

            builder.Property(e => e.CreatorRoyalty)
                .HasColumnName("creator_royality")
                .HasDefaultValue(0);

            builder.Property(e => e.OwnerId)
               .HasColumnName("owner_id");

            builder.Property(e => e.FixedPrice)
               .HasColumnName("fixed_price")
               .HasDefaultValue(false);

            builder.Property(e => e.MinBidPrice)
               .HasColumnName("min_bid_price")
               .HasDefaultValue(0);

            builder.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");

            builder.Property(e => e.ModifiedOn)
                .HasColumnName("modified_on");

            builder.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

            builder.Property(e => e.TransactionHash)
                .HasColumnName("transaction_hash");

            builder.Property(e => e.TransactionIndex)
                .HasColumnName("transaction_index");

            builder.Property(e => e.BlockHash)
                .HasColumnName("block_hash");

            builder.Property(e => e.BlockNumber)
                .HasColumnName("block_number");

            builder.Property(e => e.From)
                .HasColumnName("from");

            builder.Property(e => e.To)
                .HasColumnName("to");

            builder.Property(e => e.CumulativeGasUsed)
                .HasColumnName("cumulative_gas_used");

            builder.Property(e => e.GasUsed)
                .HasColumnName("gas_used");

            builder.Property(e => e.EffectiveGasPrice)
                .HasColumnName("effective_gas_price");

            builder.Property(e => e.MintStatus)
                .HasColumnName("mint_status");

            builder.HasOne(d => d.Status)
                .WithMany()
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Bundle)
                .WithMany(p => p.NFTs)
                .HasForeignKey(d => d.BundleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Owner)
                .WithMany(p => p.OwnedNFTs)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Creator)
                .WithMany(p => p.MintedNFTs)
                .HasForeignKey(d => d.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.AfterUpdate(trigger => trigger
                .Action(action => action
                .Condition((transactionBeforeUpdate, transactionAfterUpdate) => transactionBeforeUpdate.CurrentPrice != transactionAfterUpdate.CurrentPrice)
                .Insert((oldValues, newValues) => new NFTPriceHistory()
                {
                    ParentId = oldValues.Id,
                    Price = oldValues.CurrentPrice,
                    DateOfPrice = oldValues.CurrentPriceDate
                })));
        }
    }
}
