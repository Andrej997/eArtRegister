using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users", "eart");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.Username)
                .HasColumnName("username");

            builder.Property(e => e.PasswordHash)
                .HasColumnName("password_hash");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.Property(e => e.Surname)
                .HasColumnName("surname");
        }
    }
}
