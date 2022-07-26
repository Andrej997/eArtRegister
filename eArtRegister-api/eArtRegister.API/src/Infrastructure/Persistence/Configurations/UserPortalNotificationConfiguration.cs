using eArtRegister.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eArtRegister.API.Infrastructure.Persistence.Configurations
{
    public class UserPortalNotificationConfiguration : IEntityTypeConfiguration<UserPortalNotification>
    {
        public void Configure(EntityTypeBuilder<UserPortalNotification> builder)
        {
            builder.ToTable("user_portal_notification", "eart");

            builder.HasKey(t => t.Id)
                .HasName("user_portal_notification_pkey");

            builder.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("uuid")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired();

            builder.Property(e => e.UserId)
                .HasColumnName("user_id");

            builder.Property(e => e.Title)
                .HasColumnName("title");

            builder.Property(e => e.Description)
                .HasColumnName("description");

            builder.Property(e => e.DateOfNotification)
                .HasColumnName("date_of_notification");

            builder.Property(e => e.Seen)
                .HasColumnName("seen");

            builder.Property(e => e.SeenDate)
                .HasColumnName("seen_date");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
