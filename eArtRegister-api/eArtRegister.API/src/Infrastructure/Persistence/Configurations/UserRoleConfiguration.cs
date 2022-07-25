using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("user_role", "eart");

            builder.HasKey(e => new { e.UserId, e.RoleId });

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.RoleId)
                .HasColumnName("role_id");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Roles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Role)
                .WithMany()
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(
                new UserRole
                {
                    UserId = System.Guid.Parse("09ff8406-6775-4bf9-a5b1-d37510cc65e6"),
                    RoleId = 1
                });
        }
    }
}
