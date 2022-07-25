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

            //builder.Property(e => e.ModifiedBy)
            //    .HasColumnName("modified_by");

            //builder.Property(e => e.ModifiedOn)
            //    .HasColumnName("modified_on");

            //builder.Property(e => e.IsDeleted)
            //    .HasColumnName("is_deleted");

            builder.HasData(
                new User { 
                    Id = System.Guid.Parse("09ff8406-6775-4bf9-a5b1-d37510cc65e6"), 
                    Username = "admin",
                    Password = "$2a$11$T0F92aa6MyQHNYJERrUzge4Teh5QsO9GljSREDDuW/Y8p3YHX02Ra",
                    Name = "Admin",
                    Surname = "Admin",
                    Email = "andrej.km997@gmail.com",
                    EmailNotification = true,
                    IsDeleted = false
                });
        }
    }
}
