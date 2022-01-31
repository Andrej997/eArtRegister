using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_roles", "eart");

            builder.HasKey(e => new { e.UserId, e.RoleId });

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.RoleId)
                .HasColumnName("role_id");
        }
    }
}
