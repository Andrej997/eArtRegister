using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class UserDepositConfiguration : IEntityTypeConfiguration<UserDeposit>
    {
        public void Configure(EntityTypeBuilder<UserDeposit> builder)
        {
            builder.ToTable("user_deposit", "eart");

            builder.HasKey(t => t.Id)
                .HasName("user_deposit_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.DepositValue)
                .HasColumnName("deposit_value");

            builder.Property(e => e.DepositeDate)
                .HasColumnName("deposit_date");

            builder.Property(e => e.TransactionHash)
                .HasColumnName("transaction_hash");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Deposits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
