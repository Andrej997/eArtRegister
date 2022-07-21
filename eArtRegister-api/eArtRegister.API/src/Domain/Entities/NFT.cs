using System;

namespace eArtRegister.API.Domain.Entities
{
    public class NFT
    {
        public string Id { get; set; }
        public long StatusId { get; set; }
        public Guid BundleId { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime MintedAt { get; set; }
        public double MintedPrice { get; set; }
        public double CurrentPrice { get; set; }
        public double CreatorRoyality { get; set; }
        public Guid OwnerId { get; set; }

        public virtual NFTStatus Status { get; set; }
        public virtual Bundle Bundle { get; set; }
        public virtual User Creator { get; set; }
        public virtual User Owner { get; set; }
    }
}
