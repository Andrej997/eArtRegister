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

            builder.Property(e => e.CustomRoot)
                .HasColumnName("custom_root");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Symbol)
                .HasColumnName("symbol");

            builder.Property(e => e.Description)
                .HasColumnName("description");

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

            builder.Property(e => e.Address)
                .HasColumnName("address");

            builder.Property(e => e.Abi)
                .HasColumnName("abi");

            builder.Property(e => e.Bytecode)
                .HasColumnName("bytecode");

            builder.Property(e => e.Contract)
                .HasColumnName("contract");
        }
    }
}
