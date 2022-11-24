using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user", "eart");

            builder.HasKey(t => t.Id)
                .HasName("user_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.Wallet)
                .HasColumnName("wallet");

            builder.Property(e => e.DepositContract)
                .HasColumnName("deposit_contract");

            builder.Property(e => e.DepositAddress)
                .HasColumnName("deposit_address");

            builder.Property(e => e.DepositAbi)
                .HasColumnName("deposit_abi");

            builder.Property(e => e.DepositBytecode)
                .HasColumnName("deposit_bytecode");

            builder.Property(e => e.DepositCreated)
                .HasColumnName("deposit_created");

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
        }
    }
}
