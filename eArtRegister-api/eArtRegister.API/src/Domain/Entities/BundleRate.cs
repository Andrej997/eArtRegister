using System;

namespace eArtRegister.API.Domain.Entities
{
    public class BundleRate
    {
        public Guid BundleId { get; set; }
        public double Rate { get; set; }
        public DateTime DateOfRate { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }

        public virtual Bundle Bundle { get; set; }
        public virtual User User { get; set; }
    }
}
