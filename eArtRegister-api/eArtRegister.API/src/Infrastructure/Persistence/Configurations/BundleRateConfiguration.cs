using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class BundleRateConfiguration : IEntityTypeConfiguration<BundleRate>
    {
        public void Configure(EntityTypeBuilder<BundleRate> builder)
        {
            builder.ToTable("bundle_rate", "eart");

            builder.HasKey(t => new { t.UserId, t.BundleId })
                .HasName("bundle_rate_pkey");

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.BundleId)
                .HasColumnName("bundle_id");

            builder.Property(e => e.Rate)
                .HasColumnName("rate");

            builder.Property(e => e.DateOfRate)
                .HasColumnName("date_of_rate");

            builder.Property(e => e.Comment)
                .HasColumnName("comment");

            builder.HasOne(d => d.User)
                .WithMany(p => p.RatedBundles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Bundle)
                .WithMany(p => p.Rates)
                .HasForeignKey(d => d.BundleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
