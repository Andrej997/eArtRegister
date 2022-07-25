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

            builder.Property(e => e.Username)
                .HasColumnName("username");

            builder.Property(e => e.Password)
                .HasColumnName("password");

            builder.Property(e => e.Email)
                .HasColumnName("email");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Surname)
                .HasColumnName("surname");

            builder.Property(e => e.MetamaskWallet)
                .HasColumnName("metamask_wallet");

            builder.Property(e => e.DateAddedWallet)
                .HasColumnName("date_added_wallet");

            builder.Property(e => e.EmailNotification)
                .HasColumnName("email_notification")
                .HasDefaultValue(false);
        }
    }
}
