using System;

namespace eArtRegister.API.Domain.Entities
{
    public class FollowBundle
    {
        public Guid UserId { get; set; }
        public Guid BundleId { get; set; }
        public DateTime FollowDate { get; set; }

        public virtual User User { get; set; }
        public virtual Bundle Bundle { get; set; }
    }
}
