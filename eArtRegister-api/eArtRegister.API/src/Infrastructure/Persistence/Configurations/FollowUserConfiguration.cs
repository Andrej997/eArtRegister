using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class FollowUserConfiguration : IEntityTypeConfiguration<FollowUser>
    {
        public void Configure(EntityTypeBuilder<FollowUser> builder)
        {
            builder.ToTable("follow_user", "eart");

            builder.HasKey(t => new { t.UserId, t.FollowedUserId })
                .HasName("follow_user_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.FollowedUserId)
                .HasColumnName("followed_user_id");

            builder.Property(e => e.FollowDate)
                .HasColumnName("follow_date");

            builder.HasOne(d => d.User)
                .WithMany(p => p.FollowingUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.FollowedUser)
                .WithMany(p => p.Followers)
                .HasForeignKey(d => d.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
