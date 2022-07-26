using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class FollowBundleConfiguration : IEntityTypeConfiguration<FollowBundle>
    {
        public void Configure(EntityTypeBuilder<FollowBundle> builder)
        {
            builder.ToTable("follow_bundle", "eart");

            builder.HasKey(t => new { t.UserId, t.BundleId })
                .HasName("follow_bundle_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.BundleId)
                .HasColumnName("bundle_id");

            builder.Property(e => e.FollowDate)
                .HasColumnName("follow_date");

            builder.HasOne(d => d.User)
                .WithMany(p => p.FollowingBundles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Bundle)
                .WithMany(p => p.Followers)
                .HasForeignKey(d => d.BundleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
