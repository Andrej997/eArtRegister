using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class BundleConfiguration : IEntityTypeConfiguration<Bundle>
    {
        public void Configure(EntityTypeBuilder<Bundle> builder)
        {
            builder.ToTable("bundle", "eart");

            builder.HasKey(t => t.Id)
                .HasName("bundle_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Description)
                .HasColumnName("description");

            builder.Property(e => e.Order)
                .HasColumnName("order");

            builder.Property(e => e.IsObservable)
                .HasColumnName("is_observable");

            builder.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");

            builder.Property(e => e.ModifiedOn)
                .HasColumnName("modified_on");

            builder.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

            builder.Property(e => e.ContractAddress)
                .HasColumnName("contract_address");

            builder.Property(e => e.From)
                .HasColumnName("from");

            builder.Property(e => e.TransactionHash)
                .HasColumnName("transaction_hash");

            builder.Property(e => e.BlockHash)
                .HasColumnName("block_hash");
        }
    }
}
