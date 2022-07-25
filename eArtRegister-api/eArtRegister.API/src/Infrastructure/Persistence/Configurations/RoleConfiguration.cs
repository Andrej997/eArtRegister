using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("roles", "eart");

            builder.HasKey(t => t.Id)
                .HasName("role_pkey");

            builder.Property(e => e.Name)
                .HasColumnName("name");

            builder.HasData(
                new Role { 
                    Id = 1, 
                    Name = "Administrator"
                },
                new Role
                {
                    Id = 2,
                    Name = "Observer"
                },
                new Role
                {
                    Id = 3,
                    Name = "Buyer"
                },
                new Role
                {
                    Id = 4,
                    Name = "Seller"
                }
                );
        }
    }
}
